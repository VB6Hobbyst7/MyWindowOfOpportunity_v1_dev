/*!
 * Previewer v0.1.0
 * https://github.com/fengyuanchen/previewer
 *
 * Copyright (c) 2014-2015 Fengyuan Chen
 * Released under the MIT license
 *
 * Date: 2015-07-03T15:30:53.998Z
 */

(function (factory) {
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as anonymous module.
    define('previewer', ['jquery'], factory);
  } else if (typeof exports === 'object') {
    // Node / CommonJS
    factory(require('jquery'));
  } else {
    // Browser globals.
    factory(jQuery);
  }
})(function ($) {

  'use strict';

  var location = window.location,
      NAMESPACE = 'previewer',
      EVENT_CLICK = 'click.' + NAMESPACE,
      EVENT_LOAD = 'load.' + NAMESPACE;

  function encodeSearch(data) {
    var params = [];

    $.each(data, function (name, value) {
      params.push([name, value].join('='));
    });

    return '?' + params.join('&');
  }

  function decodeSearch(search) {
    var params = {};

    if (search) {
      search = search.replace('?', '').toLowerCase().split('&');

      $.each(search, function (i, param) {
        param = param.split('=');
        i = param[0];
        params[i] = param[1];
      });
    }

    return params;
  }

  function addTimestamp(url) {
    var timestamp = 'timestamp=' + (new Date()).getTime();

    return (url + (url.indexOf('?') === -1 ? '?' : '&') + timestamp);
  }

  function Previewer (element, options) {
    this.$element = $(element);
    this.options = $.extend({}, Previewer.DEFAULTS, $.isPlainObject(options) && options);
    this.active = false;
    this.init();
  }

  Previewer.prototype = {
    constructor: Previewer,

    init: function () {
      var options = this.options,
          params,
          type,
          show;

      this.params = params = decodeSearch(location.search);

      if (params.hasOwnProperty(NAMESPACE)) {
        show = true;
        type = params[NAMESPACE];

        if ($.inArray(type, Previewer.TYPES) !== -1){
          options.type = type;
        }
      }

      if (show || options.show) {
        this.show();
      }
    },

    show: function () {
      var options = this.options,
          $previewer;

      if (this.active) {
        return;
      }

      this.active = true;
      this.$previewer = $previewer = $(Previewer.TEMPLATE);

      $previewer
      .find('[data-preview="' + options.type + '"]')
      .addClass('active');

      $previewer
      .find('.previewer-sidebar')
      .one(EVENT_CLICK, $.proxy(this.click, this))
      .next()
      .find('iframe')
      .one(EVENT_LOAD, $.proxy(this.load, this))
      .width(options[options.type])
      .attr('src', addTimestamp(location.href));

      this.$element.addClass('previewer-open').prepend($previewer);
    },

    click: function (e) {
      var $target = $(e.target),
          data = $target.data(),
          params = this.params;

      if (data.dismiss) {
        this.hide();
      } else {
        if (!data.preview) {
          data = $target.parent().data();
        }

        if (data && data.preview) {
          params[NAMESPACE] = data.preview;
          location.search = encodeSearch(params);
        }
      }
    },

    load: function (e) {
      var $target = $(e.target);

      $target.height($target.contents().find('body').outerHeight());
    },

    hide: function () {
      var params = this.params;

      delete params[NAMESPACE];
      location.search = encodeSearch(params);
    },

    destroy: function () {
      this.hide();
    }
  };

  Previewer.DEFAULTS = {
    // Show the preview page directly
    // Type: Boolean
    show: false,

    // Default preview type
    // Type: String
    type: 'desktop',

    // Screen widths
    desktop: 1650,
    laptop: 1440,
    tablet: 1024,
    phone: 425
    //desktop: 1650,
    //laptop: 1366,
    //tablet: 768,
    //phone: 320
  };

  //Previewer.TYPES = ['phone', 'tablet', 'laptop', 'desktop'];
  Previewer.TYPES = ['desktop', 'laptop', 'tablet', 'phone'];

  Previewer.setDefaults = function (options) {
    $.extend(Previewer.DEFAULTS, options);
  };

  Previewer.TEMPLATE = (
    '<div class="previewer-container">' +
      '<div class="previewer-sidebar">' +
        '<button data-dismiss="previewer" style="display:none;" title="Close">&times;</button>' +
        '<ul class="previewer-nav">' +
            '<li><a data-preview="desktop" title="Desktop"><i></i></a></li>' +
            '<li><a data-preview="laptop" title="Laptop"><i></i></a></li>' +
            '<li><a data-preview="tablet" title="Tablet"><i></i></a></li>' +
            '<li><a data-preview="phone" title="Phone"><i></i></a></li>' +
        '</ul>' +
      '</div>' +
      '<div class="previewer-mainbody">' +
        '<iframe scrolling="yes"></iframe>' +
      '</div>' +
    '</div>'
  );

  // Save the other previewer
  Previewer.other = $.fn.previewer;

  // Register as jQuery plugin
  $.fn.previewer = function (options) {
    return this.each(function () {
      var $this = $(this),
          data = $this.data(NAMESPACE),
          fn;

      if (!data) {
        // Ignore iframes
        if (window.parent !== window.self) {
          return;
        }

        $this.data(NAMESPACE, (data = new Previewer(this, options)));
      }

      if (typeof options === 'string' && $.isFunction((fn = data[options]))) {
        fn.call(data);
      }
    });
  };

  $.fn.previewer.Constructor = Previewer;
  $.fn.previewer.setDefaults = Previewer.setDefaults;

  // No conflict
  $.fn.previewer.noConflict = function () {
    $.fn.previewer = Previewer.other;
    return this;
  };

  $(function () {
    if (/\?.*\bpreviewer\b/.test(location.search)) {
      $('body').previewer();
    }
  });
});
