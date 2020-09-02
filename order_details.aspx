<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="order_details.aspx.cs" Inherits="order_details" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #mo-wa {
            display: none;
        }

        #web {
            display: inherit;
        }

       @media screen and (max-width: 767px) {
            #mo-wa {
                display: inherit;
            }

            #web {
                display: none;
            }
        }
    </style>
    <div id="order-details">
        <div class="row">
        </div>
        <div class="row">
            <div class="col-md-12 outer-sec">
                <div id="lbladdress" runat="server">
                    <div class="col-md-4 padding">
                        <div class="address-sec">
                            <h4>Delivery Address</h4>
                            <h5 id="lbladdname" runat="server">Pratixa patel</h5>
                            <div class="ship-address">
                                <p id="lbladd" runat="server">257, Shaktinath, G.H.B-392001</p>
                                <p id="lbladdstate" runat="server">Gujarat, India</p>
                            </div>
                            <p class="number">Phone Number: <span id="lbladdmob" runat="server">7069926537</span></p>
                        </div>
                    </div>
                </div>
            <div class="col-md-4 padding">
                    <div class="order-inner-sec">
                        <h4>Order Details</h4>
                        <table>
                            <tr>
                                <td colspan="1">Order Id</td>
                                <td class="pleft" id="lblorderid" runat="server">102114</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order Date:</td>
                                <td class="pleft" id="orderdatedid" runat="server">wednesday 5, 2019</td>
                            </tr>
                            <tr>
                                <td colspan="1">Product MRP</td>
                                <td class="pleft" id="lblmrp" runat="server">Rs.450</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order Amount</td>
                                <td class="pleft" id="lbltotordeamt" runat="server">Rs.550</td>
                            </tr>
                        </table>
                    </div>
                    <%--<p>Order Id:<span>102114</span></p>
                       <p>Order Date:<span>wednesday 5, 2019</span></p>
                       <p>Order Total:<span>Rs.450</span></p>
                       <p>Order Amount Paid:<span>Rs.550</span></p>--%>
                </div>
                <div class="col-md-4 actions padding" id="disp" runat="server">
                    <h4>More Actions</h4>
                    <div style="display: flex;display:none;">
                    <div style="width: 70%;">
                        <i class="fa fa-file-text-o icon"></i>Download Invoice<span></span>
                    </div>
                    <div style="width: 30%;">
                        <input type="submit" name="Button1" value="Download" id="Button1" class="dl-btn" />
                    </div>
                    </div>
                    <div class="share" style="display:flex">
                        <div class="" style="width:70%">
                        <p>Share with your friends</p></div>

                          <div class="social-links-odetails" style="width:30%">
                              <div id="lblmsgtest" runat="server">
              <%--  <span class="whatsapp"><a class="fa fa-whatsapp" id="web" href="#" target="_blank" style="cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;"></a></span>
                  <span class="whatsapp"><a class="fa fa-whatsapp" id="mo-wa" href="#" target="_blank" style="cursor:pointer;font-size: 22px;background: #25D366;color: #fff;padding: 6px;border-radius: 4px;"></a></span>--%>
                                  </div>



                <%--<span id="fb-share-button"><i class="fa fa-facebook hide" style="cursor:pointer;font-size: 22px;background: #3b5998;padding: 6px;margin-right: 0px;color: #fff;border-radius: 4px;"></i></span>--%>
                <%--<span id="ins"><i class="fa fa-instagram" style="cursor:pointer;font-size: 22px;background: #c13584;padding: 6px;border-radius: 4px;color: #fff;"></i></span>--%>
                <%--<img class="space" src="images/whatsapp.png" />
                <img class="space" src="images/instagram.png" />--%>
            </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12 outer-sec">
                <div class="col-md-4 padding">
                    <div class="product-des">
                        <div class="col-md-12 padding-inner border">
                            <div class="col-md-4 col-sm-4 col-xs-4 padding">
                                <div id="lblimg123" runat="server">
                                    <img src="images/burfi.jpg" id="lblimg" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8 padding">
                                <p class="name" id="lblnamee" runat="server">Suleman Mithaiwala Sweets</p>
                                <p class="qty">Qty:<span id="lblqtyno" runat="server">20</span></p>
                                <div class="weight">
                                    <p>weight:<span id="lblweigh" runat="server">200-gm</span></p>
                                    <%-- <p>Price per unit:<span id="lblunit" runat="server">54654</span></p>--%>
                                </div>
                                <%-- <p class="amount">₹2000</p>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 padding" style="display: none">
                    <div class="group-detail">
                        <table>
                            <tr>
                                <td colspan="1">Group by discount</td>
                                <td class="pleft">(-) ₹2000</td>
                            </tr>
                            <tr>
                                <td colspan="1">Group by overage</td>
                                <td class="pleft">₹2000</td>
                            </tr>
                            <tr>
                                <td colspan="1">Grand Total</td>
                                <td class="pleft">₹2000</td>
                            </tr>
                            <tr>
                                <td colspan="1">Balancing Amount</td>
                                <td class="pleft">₹2000</td>
                            </tr>
                        </table>

                    </div>
                </div>

                <div class="col-md-4 padding">
                    <div class="delivered">
                        <%--<p class="deliver-date">Delivered on Nov 17, 2018</p>--%>
                        <p class="method">Payment Method : <span id="lblpayment" runat="server"></span></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
      <script>
            var fbButton = document.getElementById('fb-share-button');
            var url = window.location.href;

            fbButton.addEventListener('click', function () {
                window.open('https://www.facebook.com/sharer/sharer.php?u=' + url,
                    'facebook-share-dialog',
                    'width=800,height=600'
                );
                return false;
            });
          </script>
          <%--<script>
              function decorateWhatsAppLink() {
                  //set up the url
                  var url = 'whatsapp://send?text=';

                  //define the message text
                  var text = 'Hello!l like this product on Sosho.in and thought of sharing with you! They’ve got some amazing group buy discounts, valid only till 23 oct 2019 10:00 AM. How about we buy it together?';

                  //encode the text
                  var encodedText = encodeURIComponent(text);

                  //find the link
                  var $whatsApp = $('.whatsapp a');

                  //set the href attribute on the link
                  $whatsApp.attr('href', url + encodedText);
              }

              //call the decorator function
              decorateWhatsAppLink()
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

