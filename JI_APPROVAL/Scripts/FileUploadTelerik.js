(function () {

    var $;
    var FileHandler = window.FileHandler = window.FileHandler || {};

    FileHandler.initialize = function () {
        $ = $telerik.$;
    };

    var ARR_filesSize = new Array();
    
    window.file_approval_Uploaded = function (sender, args) {
           
        var id = args.get_fileInfo().ImageID;
        var file = '../FileUploads/Temp/' + args.get_fileName();//args.get_fileInfo().FileNameResult;

        //$(".imageContainer")
        //    .html("")
        //    .append($("<img />")
        //    .css("max-width", "300px")
        //    .attr("src", file));

       // alert(args.get_fileName());
        //alert(args.get_fileInfo().FileNameResult);



        //AddItem(args.get_fileName());  Because we are removing the row on the control, once we do the postback the names never change
        // So we need to keep the temporary_name
        AddItem(args.get_fileInfo().FileNameResult); //Using the handdler we can keep the temporary name here
                
        //changeUpload(args.get_fileInfo().FileNameResult);
        hasFiles("true");
        sender.deleteFileInputAt(0);
        sender.set_enabled(false);

        div_Control(true);

        Refresh_DOC_grid();

        //$(".info").html(String.format("<strong>{0}</strong> successfully inserted.<p>Record ID - {1}</p>", args.get_fileName(), id));

    };

    window.fileUploaded = function (sender, args) {
        var id = args.get_fileInfo().ImageID;
        var file = "../Temp/" + args.get_fileInfo().FileNameResult;

        //$(".imageContainer")
        //    .html("")
        //    .append($("<img />")
        //    .css("max-width", "300px")
        //    .attr("src", file));
        
        changeUpload(args.get_fileInfo().FileNameResult);
        hasFiles("true");
        //$(".info").html(String.format("<strong>{0}</strong> successfully inserted.<p>Record ID - {1}</p>", args.get_fileName(), id));

    };
        
    window.APP_fileUploaded = function (sender, args) {

        //var id = args.get_fileInfo().ImageID;
        var file_NewName = args.get_fileInfo().fileNameResult;

        //alert(file);

        //$(".imageContainer")
        //    .html("")
        //    .append($("<img />")
        //    .css("max-width", "300px")
        //    .attr("src", file));        

        alert('Hello changeUpload');
        changeUpload(file_NewName);
        //alert('Bye changeUpload');
        hasFiles("true");
        //print('upload');
        showElements(true);

        //$(".info").html(String.format("<strong>{0}</strong> successfully inserted.<p>Record ID - {1}</p>", args.get_fileName(), id));

    };

    window.getImageUrl = function (id) {
        var url = window.location.href;
        var handler = "StreamImage.ashx?imageID=" + id;
        var index = url.lastIndexOf("/");
        var completeUrl = url.substring(0, index + 1) + handler;
        return completeUrl
    };

    window.fileUploadRemoving = function (sender, args) {
        var index = args.get_rowIndex();
        $(".imageContainer img:eq(" + index + ")").remove();
        hasFiles("false");
    };
    
    window.APP_fileUploadRemoving = function (sender, args) {
                
        //alert(sender + ' ' + args.get_rowIndex() + ' ' + args.get_fileName());
        hasFiles("false");
        //var index = args.get_rowIndex();
        //alert(sender + ' ' + args.get_rowIndex() + ' ' + args.get_fileName());
        //sender.deleteFileInputAt(index);
        //if (args.get_rowIndex() == 0) {
        //    args.set_cancel(true);
        //}              
        showElements(false);

    };

    
    window.onClientFileUploading = function (sender, args) {
        
        var data = args.get_data();
        //var percents = data.percent;
        var fileSize = data.fileSize;
        var fileName = data.fileName;
        //var uploadedBytes = data.uploadedBytes;
        //alert("Uploading information: File name: " + fileName + ", " + percents + "% completed, total size: " + fileSize + " , uploaded: bytes " + uploadedBytes + ".");        
        ARR_filesSize[args.get_data().fileName] = fileSize;
       // alert(fileName + ': ' + fileSize);

    };

    //******************************VALIDATION PART***********************************//
    window.validationFailed = function (radAsyncUpload, args) {

        //alert(args.get_row()); //.get_row()
        //var $row = $(args.get_row());
        var $row = $telerik.$(args.get_row());
        var errorMessage = getErrorMessage(radAsyncUpload, args);
        //alert(errorMessage);
        var span = createError(errorMessage);
        $row.addClass("ruError");
        $row.append(span);
        //setTimeout(function () { sender.deleteFileInputAt(0); }, 10);

    };


    function OnClientValidationFailed__test(sender, args) {
        try {
            var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
            if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) { //alert("Wrong Extension!");
                    var msg = 'The file upload has failed because this is not an allowed extension. Allowed extensions are {0}.';
                    msg = msg.replace("{0}", sender.get_allowedFileExtensions());
                    radalert(msg, 400, 200, 'The file upload has failed.', null, null);
                }
                else { //alert("Wrong file size!");
                    var msg = 'The file upload has failed because the file is to large. Maximum file size allowed is {0} Kb.'
                    msg = msg.replace("{0}", 4096000 / 1024);
                    radalert(msg, 400, 200, 'The file upload has failed.', null, null);
                }
            }
            else { //alert("not correct extension!");
                var msg = 'The file upload has failed because this is not an allowed extension. Allowed extensions are {0}.';
                msg = msg.replace("{0}", sender.get_allowedFileExtensions());
                radalert(msg, 400, 200, 'The file upload has failed.', null, null);
            }
        }
        catch (ex) {
            alert("JS Error #1311161741:" + ex);
        }

        setTimeout(function () { sender.deleteFileInputAt(0); }, 10);
    }

    function getErrorMessage(sender, args) {
        var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);

        var f_size = 0;

        if (typeof ARR_filesSize[args.get_fileName()] === 'undefined') {
            f_size = 0;
        } else {
            f_size = ARR_filesSize[args.get_fileName()];
        }

        // alert('Allowed: ' + sender.get_allowedFileExtensions() + ' Extention: ' + fileExtention);
        //alert('File Size: ' + ARR_filesSize[args.get_fileName()] + ' Max Allowed: ' + sender._maxFileSize);

         if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct

             if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {

                        return ("This file type '" + fileExtention + "' is not supported.");

             } else {

                 var msg ='';
                 if (f_size == 0){
                     msg = "This file exceeds the maximum allowed size " + sender._maxFileSize ;
                 }else{
                     msg = "This file exceeds the maximum allowed size " + sender._maxFileSize ;+ " with " + ARR_filesSize[args.get_fileName()];
                 }

               //  alert('size: ' + f_size);

                 if (f_size > sender._maxFileSize || f_size == 0) {
                            
                            return (msg);

                 } else {

                            return ("not correct extension.");

                 }

             }

         }
               

    }

            function createError(erorMessage) {
                var input = '<br \><span class="ruErrorMessage">' + erorMessage + ' </span>';
                return input;
            }


    //******************************VALIDATION PART***********************************//




})();