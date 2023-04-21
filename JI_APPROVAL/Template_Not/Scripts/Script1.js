
(function () {
    var $;
    var demo = window.demo = window.demo || {};

    demo.initialize = function () {
        $ = $telerik.$;
    };

    window.validationFailed = function (radAsyncUpload, args) {
        var $row = $(args.get_row());
        var erorMessage = getErrorMessage(radAsyncUpload, args);
        var span = createError(erorMessage);
        $row.addClass("ruError");
        $row.append(span);
    }

    function getErrorMessage(sender, args) {
        var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
        if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
            if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                return ("This file type is not supported.");
            }
            else {
                return ("This file exceeds the maximum allowed size of 500 KB.");
            }
        }
        else {
            return ("not correct extension.");
        }
    }

    function createError(erorMessage) {
        var input = '<span class="ruErrorMessage">' + erorMessage + ' </span>';
        return input;
    }



})();