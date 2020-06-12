function ScrollToTop() {
    //Obtain anchor tag
    var aTag = $("a[name='topAnchor']");
    //Go to anchor tag
    $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
}

function ScrollToAnchor(name) {
    //Obtain anchor tag
    var aTag = $("a[name='" + name + "']");
    //Go to anchor tag
    $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
}