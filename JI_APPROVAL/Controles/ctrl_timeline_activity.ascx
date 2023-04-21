<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrl_timeline_activity.ascx.vb" Inherits="RMS_APPROVAL.ctrl_timeline_activity" %>
  <link rel="stylesheet" href="../Content/timeline.css?ts=0.014">

<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <%--class="col-lg-12 col-md-12 col-sm-12"--%>
         <div class="col-lg-12 col-md-12 col-sm-12" style="display:inline-block;width:100%;overflow-y:auto; border:0px solid #d3c8c8;" >         
                
      <%--   <div class="row">
            <div class="col-md-12">--%>
               <div class="main-timeline12">
                   <asp:Repeater ID="rpt_timeline_odd" runat="server">
                    <ItemTemplate>                         
                         <div class="timeline2">
                            <span class="timeline-icon">
                               <a href="<%# Eval("TIMELINE_URL") %>"> <i class="<%# Eval("TIMELINE_ICON") %>" title="<%# Eval("TIMELINE_STATUS") %> on <%# getFecha(Eval("TIMELINE_DATE"), "f", True) %> by <%# Eval("TIMELINE_USER") %>"></i> </a>
                            </span>
                            <div class="border"></div>
                            <small class="label pull-right <%# If(Not IsDBNull(Eval("BCHANGE")), "bg-red", "") %>">&nbsp;&nbsp;<i class='fa <%# If(Not IsDBNull(Eval("BCHANGE")), "fa-info", "") %>'></i>&nbsp;&nbsp;</small>
                            <div class="timeline-content">
                                <h4 class="title"><%# Eval("TIMELINE_STATUS") %></h4>
                                <p class="description"><%# Eval("TIMELINE_TEXT") %><br />                              
                                </p>
                                <b><%# getFecha(Eval("TIMELINE_DATE"), "D", False) %>&nbsp;<i class='fa fa-clock-o'></i>&nbsp;<%# getHora(Eval("TIMELINE_DATE")) %></b>
                                <span class='label  <%# If(Not IsDBNull(Eval("BCHANGE")), "label-success text-center", "") %> text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;<%# If(Not IsDBNull(Eval("BCHANGE")), Func_Unit(Eval("TIMELINE_DATE"), Date.UtcNow), "") %> </span>
                            </div>
                         </div>                        
                         <asp:Repeater ID="rpt_timeline_whole" runat="server">
                           <ItemTemplate> 
                                  <div class="timeline2">    
                                       <small class="label pull-right <%# If(Not IsDBNull(Eval("BCHANGE")), "bg-red", "") %>">&nbsp;&nbsp;<i class='fa <%# If(Not IsDBNull(Eval("BCHANGE")), "fa-info", "") %>'></i>&nbsp;&nbsp;</small>
                                       <div class="timeline-content">
                                            <h4 class="title"><%# Eval("TIMELINE_STATUS") %></h4>
                                            <p class="description"><%# Eval("TIMELINE_TEXT") %><br />                                            
                                            </p>   
                                             <b><%# getFecha(Eval("TIMELINE_DATE"), "D", False) %>&nbsp;<i class='fa fa-clock-o'></i>&nbsp;<%# getHora(Eval("TIMELINE_DATE")) %></b>
                                             <span class='label  <%# If(Not IsDBNull(Eval("BCHANGE")), "label-success text-center", "") %> text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;<%# If(Not IsDBNull(Eval("BCHANGE")), Func_Unit(Eval("TIMELINE_DATE"), Date.UtcNow), "") %> </span>
                                        </div>                                      
                                        <div class="border"></div>
                                        <span class="timeline-icon">
                                             <a href="<%# Eval("TIMELINE_URL") %>"> <i class="<%# Eval("TIMELINE_ICON") %>" title="<%# Eval("TIMELINE_STATUS") %> on <%# getFecha(Eval("TIMELINE_DATE"), "f", True) %> by <%# Eval("TIMELINE_USER") %>"></i> </a>
                                        </span>
                                    </div>                               
                           </ItemTemplate>
                        </asp:Repeater>                       
                    </ItemTemplate>
                 </asp:Repeater>
               </div>

              <%--  <div class="main-timeline12">
                    <div class="timeline2">
                        <span class="timeline-icon">
                           <a href="frm_activity"> <i class="fa fa-flag-checkered" title="ACTIVITY CREATED on Sexta-feira, 4 de dezembro de 2020 17:04 UTC -3 by Gustavo Rivera"></i> </a>
                        </span>
                        <div class="border"></div>
                        <small class="label pull-right bg-red">&nbsp;&nbsp;<i class='fa fa-info'></i>&nbsp;&nbsp;</small>
                        <div class="timeline-content">
                            <h4 class="title">ACTIVITY CREATED</h4>
                            <p class="description">Activity created, it's ready for solicitation.<br />                              
                            </p>
                            <b>Sexta-feira, 4 de dezembro de 2020&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3</b>
                            <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                        </div>
                    </div>
                    <div class="timeline2">                       
                        <div class="timeline-content">
                            <h4 class="title">SOLICITATION CREATED</h4>
                            <p class="description">Solicitation created, ready to request for proporsal/aplication.<br />
                             <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>                            
                        </div>
                        <div class="border"></div>
                            <span class="timeline-icon">
                                <i class="fa fa-folder"></i>
                            </span>
                    </div>
                    <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-send"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">SOLICITATION SENT</h4>
                            <p class="description">Solicitation sent to applicants.<br />
                              Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                    </div>
                    <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">SOLICITATION OPENED</h4>
                            <p class="description">The solicitation was received and opened by the potencial grantee.<br />
                              Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-folder-open"></i>
                        </span>
                    </div>
                    <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-pencil-square-o"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">APLICATION CREATED</h4>
                            <p class="description">Aplication has been created.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>
                    <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">APPLICATION PRESENTED</h4>
                            <p class="description">The applicants have presented the proporsal, waiting for the screening and approval.<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-files-o"></i>
                        </span>
                    </div>
                     <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-check-square"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">APLICATION ACEPTED</h4>
                            <p class="description">2 aplication has been acepted, remaining 3 on screeening.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>
                     <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">EVALUATION OPENED</h4>
                            <p class="description">The evaluation is opened for 3 applications.<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-folder-open-o "></i>
                        </span>
                    </div>
                    <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-check-square"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">APLICATION ON EVALUATION</h4>
                            <p class="description">3 applications are under evaluation, waiting for a choice.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>
                     <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">APLICATION SELECTED</h4>
                            <p class="description">The aplication of (Parthner name) has been selected, the activity is on award proccess.<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-legal"></i>
                        </span>
                    </div>
                     <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-files-o"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">ON AWARD PROCESS</h4>
                            <p class="description">The application of (Parthner name), is on award proccess.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>
                    <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">APLICATION AWARDED</h4>
                            <p class="description">The GRANT/SUBCONTRACT of (Parthner name) has been awarded with the code GRA-LIF-2020-0001, the activity is ready to execute.<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-book"></i>
                        </span>
                    </div>
                     <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-cogs"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">ON EXECUTION</h4>
                            <p class="description">The GRANT/SUBCO (Parthner name), is on award proccess.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>
                    <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">GRANT/SUB EXPIRED</h4>
                            <p class="description">The GRANT/SUBCONTRACT activity of (Parthner name) with the code GRA-LIF-2020-0001 is expired, it's ready to start the Close up process.<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-hourglass-end"></i>
                        </span>
                    </div>
                     <div class="timeline2">
                        <span class="timeline-icon">
                            <i class="fa fa-archive"></i>
                        </span>
                        <div class="border"></div>
                        <div class="timeline-content">
                            <h4 class="title">GRANT/SUB CLOSING UP</h4>
                            <p class="description">The GRANT/SUBCO activity of (Parthner name), is on closing up proccess.<br />
                               <span class='label label-success text-center text-sm pull-right' ><i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;2 days</span>
                            </p>
                        </div>
                    </div>

                      <div class="timeline2">
                        <div class="timeline-content">
                            <h4 class="title">GRANT/SUB CLOSED</h4>
                            <p class="description">The GRANT/SUBCONTRACT activity of (Parthner name) is closed<br />
                             Sexta-feira, 4 de dezembro de 2020&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;17:04 UTC -3
                            </p>
                        </div>
                        <div class="border"></div>
                        <span class="timeline-icon">
                            <i class="fa fa-archive"></i>
                        </span>
                    </div>

                </div>--%>
     
      </div>


    </ContentTemplate>
</asp:UpdatePanel>