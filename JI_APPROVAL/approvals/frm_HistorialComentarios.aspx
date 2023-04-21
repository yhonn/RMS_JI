<%@ Page Language="VB" MasterPageFile="~/Site.Master"   AutoEventWireup="false"    Inherits="RMS_APPROVAL.OrgChart.IntegrationWithRadToolTip.frm_HistorialComentarios" Codebehind="frm_HistorialComentarios.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
        
     <section class="content-header">
          <h1>
             <asp:Label runat="server" ID="lblt_titulo_pantalla">Environmental Approval</asp:Label>
          </h1>
     </section>
     <section class="content">

         <div class="box">

                 <div class="box-header with-border">
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Approval Flow</asp:Label>
                         <asp:HiddenField runat="server" ID="h_Filter" Value="0" />  
                    </h3>
                </div>

                <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-4 text-left">
                                         <!--Tittle -->                                         
                                            <asp:Label ID="lblIDocumento0" runat="server"  Visible="false"></asp:Label>
                                            <asp:Label ID="lblIDocumento" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblIDocumento1" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblAppIDocumento" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblIdRuta" runat="server" Visible="false"></asp:Label>
                                        </div>
                                       <div class="col-sm-8  text-right"">
                                           <!--Control -->
                                             <asp:Label ID="lbl_IdCodigoAPP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                             <asp:Label ID="lbl_categoria1" runat="server"  CssClass="control-label text-bold">/</asp:Label>
                                             <asp:Label ID="lbl_IdCodigoSAP" runat="server" CssClass="control-label text-bold"></asp:Label><br />
                                           <asp:Label ID="lbl_status" runat="server"  CssClass="control-label text-bold"></asp:Label>
                                       </div>
                                    </div>
                                  </div>
                               </div> 
              
                          </div>
                    </div>
             

                    <div class="box-body">

                            <div class="col-lg-12">

                                <div class="box-body">
                       
                                        <div class="row">
                                          <div class="col-sm-4 text-left">
                                            <!--Tittle -->
                                          <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                          </div>
                                          <div class="col-sm-7">
                                          <!--Control -->
                                          <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>                                                                        
                                          </div>
                                         </div>


                                                <div class="row">
                                                     <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                     <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Approvals"></asp:Label>                                
                   
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>                                                                                           
                                                                   </div>
                                                                 </div>

                                                                 <div class=" row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>

                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>                                                                        
                                                                   </div>
                                                                 </div>

                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                       <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>                                                                        
                                                                   </div>
                                                                 </div>
                                                   
                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                       <asp:Label ID="lblt_tipoDocumento" runat="server"  CssClass="control-label text-bold" Text="Document Type"></asp:Label>                                                        
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_tipoDocumento" runat="server" ></asp:Label>                                                                                                                                          
                                                                   </div>
                                                                 </div>

                                                                  <div class="row">
                                                                        <div class="col-sm-4 text-left">
                                                                         <!--Tittle -->
                                                                          <asp:Label ID="lblt_proccess_name" runat="server"   CssClass="control-label text-bold" Text="Name of Process"></asp:Label>
                        
                                                                        </div>
                                                                       <div class="col-sm-7">
                                                                           <!--Control -->
                                                                           <asp:Label ID="lbl_proceso" runat="server" style="font-weight: 700"></asp:Label>
                                                                       </div>
                                                                 </div>      

                                                                 
                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->                                                    
                                                                        <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Code of Process"></asp:Label>                    
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->                                                                   
                                                                        <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>   

                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->  
                                                                          <asp:Label ID="lblt_instrument_number" runat="server"  CssClass="control-label text-bold"  Text="Instrument Number"></asp:Label>                                                  
                                                                    
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_instrumento" runat="server"></asp:Label>
                                                   
                                                                   </div>
                                                                 </div>
                                                                
                                            
                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_beneficiary" runat="server"  CssClass="control-label text-bold"  Text="Beneficiary Name"></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                         <asp:Label ID="lbl_beneficiario" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>
                                                   
                                                                  <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Datedcreated" runat="server"  CssClass="control-label text-bold" Text="Date Created"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_datecreated" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>


                                                               <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->                                                    
                                                                           <asp:Label ID="lblt_dateApproved" runat="server"  CssClass="control-label text-bold" Text="Date Approved"></asp:Label>                                                        
                                                                    
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_dateapproved" runat="server"  ></asp:Label>
                                                                       <asp:Label ID="lbl_ErrApprovedBy" runat="server" CssClass="control-label text-bold"  ToolTip="Date of receipt- Pending">***</asp:Label>
                                                   
                                                                   </div>
                                                               </div>
                                                                                                                   
                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_createdBy" runat="server"  CssClass="control-label text-bold" Text="Created By"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_createdby" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>
                                                               

                                                               <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_approvedBy" runat="server"  CssClass="control-label text-bold" Text="Approved By"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                           <asp:Label ID="lbl_approvedby" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="tblt_status" runat="server"  CssClass="control-label text-bold" Text="Estado"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                             <asp:Label ID="lbl_status2" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                   <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lbl_comment" runat="server"  CssClass="control-label text-bold" Text="Comentarios"></asp:Label>                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->                                                                          
                                                                         <telerik:RadButton ID="btn_guardar" runat="server" Text=" comments >>" Height="25px" Width="150px"  ></telerik:RadButton><br />

                                                                   </div>
                                                                 </div>



                                              </div>
                                                              
                                </div>

                          
                           <div class="box-body">
                            <div class="col-lg-12">



                                <div class="box-body">
                                                                        
                                    <telerik:RadOrgChart  
                                        ID="RadOrgChartML"
                                        runat="server" 
                                        DisableDefaultImage="True" 
                                        Skin="Office2010Blue" 
                                        Font-Names ="Arial" 
                                        Width="95%"  >

                                      <RenderedFields>
                                        <NodeFields >
                                            <telerik:OrgChartRenderedField DataField="Titulo" />                                            
                                        </NodeFields>
                                        <ItemFields  >
                                            <telerik:OrgChartRenderedField DataField="Detalle"  />                                                                                                                                    
                                       </ItemFields>
                                      </RenderedFields>
                               
                                    </telerik:RadOrgChart> 

                                     <telerik:RadToolTipManager runat="server" ID="RadToolTipManager1" Skin="Default"
                                        Position="BottomRight" OffsetX="16" OffsetY="16" EnableShadow="false" Width="195" Height="135" />

                                 </div>

                             </div>
                           </div>
            
                    </div>
              
            
             
              </div> <!-- Box -->
         
         <style type="text/css"> 


                  /*html .RadOrgChart .rocItemContent,
                  html .RadOrgChart .rocItemTemplate {
                        width:500px !important;
                        height:200px !important;
                        padding: 0 0 0 0;
                  }


                   html .RadOrgChart .rocItem {
                      width:500px !important;
                       
                    }*/

                  html .RadOrgChart .rocItemContent  {
                           /*background-color:red;*/
                           width:400px !important;
                           height:150px !important;                           
                           text-align:justify;
                           position:absolute;
                             padding: 0 0 0 0 !important;     
                           top:0px;
                           left:0px;
                           bottom:10px;
                           
                        }


                .newItemSizeSimple
                {
                    width:700px !important;
                    height:200px !important;
                    padding: 0 0 0 0 !important;        
                    /*border: 1px solid black;*/             
                }

                /*.newItemSizeSimple1
                {
                    border: 1px solid black;   
                     /*padding: 0 0 0 0 !important;
                     height:200px !important;*/     
                /*}*/

            </style>

      </section>            

        
    </asp:Content>


                  
         
                   

    
 