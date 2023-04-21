(function (global, undefined) {
    var demo = {};

    function OnClientFilesUploaded(sender, args) {
        $find(demo.ajaxManagerID).ajaxRequest();
    }

    function serverID(name, id) {
        demo[name] = id;
    }
    global.serverID = serverID;
    global.OnClientFilesUploaded = OnClientFilesUploaded;

})(window);
