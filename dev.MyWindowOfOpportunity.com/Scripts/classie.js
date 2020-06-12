/*!
 * classie - class helper functions
 * from bonzo https://github.com/ded/bonzo
 * 
 * classie.has( elem, 'my-class' ) -> true/false
 * classie.add( elem, 'my-new-class' )
 * classie.remove( elem, 'my-unwanted-class' )
 * classie.toggle( elem, 'my-class' )
 */
function pageLoad() {
    (function (n) {

        'use strict';

        // class helper functions from bonzo https://github.com/ded/bonzo

        function classReg(className) {
            return new RegExp("(^|\\s+)" + className + "(\\s+|$)");
        }

        // classList support for class management
        // altho to be fair, the api sucks because it won't accept multiple classes at once
        var hasClass, addClass, removeClass;

        if ('classList' in document.documentElement) {
            hasClass = function (elem, c) {
                return elem.classList.contains(c);
            };
            addClass = function (elem, c) {
                elem.classList.add(c);
            };
            removeClass = function (elem, c) {
                elem.classList.remove(c);
            };
        }
        else {
            hasClass = function (elem, c) {
                return classReg(c).test(elem.className);
            };
            addClass = function (elem, c) {
                if (!hasClass(elem, c)) {
                    elem.className = elem.className + ' ' + c;
                }
            };
            removeClass = function (elem, c) {
                elem.className = elem.className.replace(classReg(c), ' ');
            };
        }

        function toggleClass(elem, c) {
            var fn = hasClass(elem, c) ? removeClass : addClass;
            fn(elem, c);
        }

        var classie = {
            // full names
            hasClass: hasClass,
            addClass: addClass,
            removeClass: removeClass,
            toggleClass: toggleClass,
            // short names
            has: hasClass,
            add: addClass,
            remove: removeClass,
            toggle: toggleClass
        };

        // transport
        if (typeof define === 'function' && define.amd) {
            // AMD
            define(classie);
        } else if (typeof exports === 'object') {
            // CommonJS
            module.exports = classie;
        } else {
            // browser global
            window.classie = classie;
        }

    })(window),
    function () {
        function n(n) {
            //setTimeout(classie.add(n.target.parentNode, "input--filled"), 3000)
            classie.add(n.target.parentNode, "input--filled")
        }

        function t(n) {
            //setTimeout(n.target.value.trim() === "" && classie.remove(n.target.parentNode, "input--filled"), 3000)
            n.target.value.trim() === "" && classie.remove(n.target.parentNode, "input--filled")
        }
        String.prototype.trim || function () {
            var n = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(n, "")
            }
        }();
        [].slice.call(document.querySelectorAll("input.input__field, textarea.input__field")).forEach(function (i) {
            //setTimeout(function () {
            i.value.trim() !== "" && classie.add(i.parentNode, "input--filled");
            i.addEventListener("focus", n);
            i.addEventListener("blur", t)
            //},2000);
        })
    }();
}
