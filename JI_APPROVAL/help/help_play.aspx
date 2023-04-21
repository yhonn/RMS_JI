<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="help_play.aspx.vb" Inherits="RMS_APPROVAL.help_play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    
    <div style="padding-left:2px;padding-top:2px; vertical-align:top;" class="form-group row ">
          <video id="vid_help" width="985" height="700" style="object-fit: fill;"  controls >
              <source src="" type="video/mp4" />                     
              Your browser does not support the video tag.
         </video>
     </div>

</body>
  
    <script src="<%=ResolveUrl("~/Content/Jquery/jQuery-2.1.4.min.js")%>"></script>
    <script type="text/javascript">

        $(document).ready(function(){
            
                    var params = getSearchParameters();

                    //params.video_help              
                    var path = "../help/videos/" + params.video_help;
                    //console.log(path);

                    $('source').attr('src', path);
                    $('video#vid_help')[0].load();
                    $('video#vid_help')[0].play();       
                    //$('#vid_help')[0].play();

        });


              function getSearchParameters() {
                      var prmstr = window.location.search.substr(1);
                      return prmstr != null && prmstr != "" ? transformToAssocArray(prmstr) : {};
              }

            function transformToAssocArray( prmstr ) {
                var params = {};
                var prmarr = prmstr.split("&");
                for ( var i = 0; i < prmarr.length; i++) {
                    var tmparr = prmarr[i].split("=");
                    params[tmparr[0]] = tmparr[1];
                }
                return params;
            }


    </script>
</html>
