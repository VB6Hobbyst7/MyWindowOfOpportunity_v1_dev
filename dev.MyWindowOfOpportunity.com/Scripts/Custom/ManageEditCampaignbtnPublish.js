// Manage Edit Campaign Publish Btn Event 
//========================================


function btnPublish() {
    $.Zebra_Dialog('Once published, a campaign cannot be unpublished.  Phases and categories cannot be updated after publishing either.<br><br> <strong> Are you ready to publish your campaign?</strong>', {
        'type': 'warning', //confirmation, information, warning, error and question
        'title': 'Publish Campaign?',
        'buttons': [
            {
                caption: 'Yes', callback: function () {
                    btnPublish_Click();
                }
            },
            { caption: 'No' }
        ]
    });
}

function btnPublish_Click() {
    var btnPub = $('.btnPublishHidden');
    btnPub.trigger("click");
}

//function btnUnPublish() {
//    $.Zebra_Dialog('Are you sure you want to unpublish your campaign?.<br><br> <strong> Unpublishing a campaign will also delete any existing pledges. Continue?</strong>', {
//        'type': 'warning', //confirmation, information, warning, error and question
//        'title': 'Unpublish Campaign?',
//        'buttons': [
//            {
//                caption: 'Yes', callback: function () {
//                    btnUnPublish_Click();
//                }
//            },
//            { caption: 'No' }
//        ]
//    });
//}

//function btnUnPublish_Click() {
//    var btnUnpub = $('.btnUnPublishHidden');
//    btnUnpub.trigger("click");
//}