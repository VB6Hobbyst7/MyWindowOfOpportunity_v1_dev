//Run Zurb Foundation with Equalizer
$(document).foundation({
    equalizer: {
        equalize_on_stack: true, // Specify if Equalizer should make elements equal height once they become stacked.
        act_on_hidden_el: true // Allow equalizer to resize hidden elements
    },
    accordion: {
        // specify the class used for accordion panels
        content_class: 'content',
        // specify the class used for active (or open) accordion panels
        active_class: 'active',
        // allow multiple accordion panels to be active at the same time
        multi_expand: false,
        // allow accordion panels to be closed by clicking on their headers
        // setting to false only closes accordion panels when another is opened
        toggleable: true
    }
});

//Run svg4Everybody js plugin
svg4everybody(); 



$(function () {
    try {
        //Page title update
        if ($('#campaignPg').length > 0) {
            $(document).prop('title', 'MWoO | ' + $('.campaignTitle').text());
        }
    }
    catch (err) {
        console.log('ERROR: [FinalScripts- Page Title] ' + err.message + ' | ' + err);
    }
});