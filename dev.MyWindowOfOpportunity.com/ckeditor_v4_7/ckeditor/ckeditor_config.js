/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

//CKEDITOR.editorConfig = function (config) {
//    // Define changes to default configuration here. For example:
//    // config.language = 'fr';
//    // config.uiColor = '#AADC6E';
//    config.skin = 'icy_orange';
//    config.extraPlugins = "imagebrowser";
//    config.imageBrowser_listUrl = "/ckeditor_v4_7/browserImageList_campaign.json";
//    //config.extraPlugins = 'basicstyles';
//    config.removeButtons = "Save";
//};


CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'insert', groups: ['insert'] },
		'/',
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
		{ name: 'links', groups: ['links'] },
		'/',
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Save,NewPage,Preview,Print,Templates,Form,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,CopyFormatting,Language,BidiRtl,BidiLtr,Flash,About,Checkbox';

    config.skin = 'icy_orange';
    config.extraPlugins = "imagebrowser";
    config.imageBrowser_listUrl = "/ckeditor_v4_7/browserImageList_campaign.json";
    config.allowedContent = true;
    config.extraAllowedContent = '*[id](*)';
    config.height = '600px';
};