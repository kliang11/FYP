<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="FYP.Project.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />

    <div class="profilePanel">

        <header class="pb-2 fixed-profile-header" style="border-bottom: 1px solid #c9c9c9;">
            <asp:Label ID="lblTitle" runat="server" Text="Staff" CssClass="alignleft mt-2"></asp:Label>
            <asp:LinkButton ID="btnEditt" runat="server" CssClass="alignright btn btn-danger" OnClientClick="return showUploadFile()" OnClick="btnEditt_Click" CausesValidation="False"><i class="fas fa-edit"></i> Edit </asp:LinkButton>
            <asp:LinkButton ID="btnSave" runat="server" CssClass="alignright btn btn-success" OnClientClick="return hideUploadFileDeleteButton()" OnClick="btnSave_Click" Visible="False"><i class="fas fa-save"></i> Save </asp:LinkButton>
            <asp:LinkButton ID="btnBack" runat="server" CssClass="alignright btn btn-light"  CausesValidation="False" OnClick="btnBack_Click"><i class="fa fa-angle-left"></i> Back </asp:LinkButton>
            <div style="clear: both;"></div>
        </header>

        <asp:Panel ID="personalPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Personal</label>

            <asp:Image ID="imgProfile" runat="server" class="profileImage" alt="profileImage" ClientIDMode="Static" ImageUrl="~/Image/defaultProfileImg.png" />
            <div style="text-align: center; margin-bottom: 1%;">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Image must be in .jpg, .jpeg, .png format only." ForeColor="Red" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG)$" ControlToValidate="FileUpload1"></asp:RegularExpressionValidator>
            </div>

            <div style="text-align: center; position: relative;">
                <asp:LinkButton ID="btnDeletes" runat="server" CssClass="btn btn-light mb-2" OnClientClick="hideDeleteButton()" OnClick="btnDelete_Click" CausesValidation="False" style="display: inline-block;"><i class="fas fa-save"></i> Delete </asp:LinkButton>
                <%--<span class="hint">Delete</span>--%>
            </div>

            <label id="lblFileUpload" class="file-upload">
                <span><strong>Upload Image</strong></span>
                <asp:FileUpload ID="FileUpload1" runat="server" onchange="showimagepreview(this)" accept="image/*" name="image"></asp:FileUpload>
            </label>


            <div class="form-group">
                <label>Staff ID</label>
                <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control" placeholder="Staff ID*" Enabled="False"></asp:TextBox>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">
                        Full Name 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required" ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                    <label class="column">
                        Email 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email Required" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control column" placeholder="Full Name*" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control column" placeholder="Email*" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Gender</label>
                    <label class="column">
                        Nationality 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nationality Required" ControlToValidate="txtNationality" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control column" Style="margin-right: 2%">
                        <asp:ListItem>Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                        <asp:ListItem>Unknown</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control column" placeholder="Nationality* - eg. Malaysia" onkeydown="return (((event.keyCode<=90 && event.keyCode>=65) || (event.keyCode<=122 && event.keyCode>=97)) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">
                        NRIC 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="NRIC Required" ControlToValidate="txtIC" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                    <label class="column">
                        Phone Number
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Phone Number Required" ControlToValidate="txtPhoneNo" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtIC" runat="server" CssClass="form-control column" placeholder="NRIC* (number only)" Style="margin-right: 2%" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                    <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control column" placeholder="Phone Number* (number only)" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Address
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Address Required" ControlToValidate="txtAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                </label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address*"></asp:TextBox>
            </div>
            <label class="subtitle">Family</label>



            <div class="form-group">
                <label>Marital Status</label>
                <asp:DropDownList ID="ddlMaritalStatus" runat="server" onchange="maritalstatuschange(this)" CssClass="form-control" OnSelectedIndexChanged="ddlMaritalStatus_SelectedIndexChanged">
                    <asp:ListItem>Single</asp:ListItem>
                    <asp:ListItem>Married</asp:ListItem>
                    <asp:ListItem>Divorce/Widow/Widower</asp:ListItem>
                </asp:DropDownList>
            </div>


            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Spouse Working</label>
                    <label class="column">
                        Number of Children
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Number of Child Required" ControlToValidate="txtNoChild" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlSpouseWork" runat="server" CssClass="form-control column" Style="margin-right: 2%" Enabled="False" ClientIDMode="Static">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtNoChild" runat="server" CssClass="form-control column" placeholder="Number of Child* - eg. 2" Enabled="False" Text="0" TextMode="Number" min="0" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Spouse Disable</label>
                    <label class="column">Self Disable</label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlSpouseDisable" runat="server" CssClass="form-control column" Style="margin-right: 2%" Enabled="False" ClientIDMode="Static">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSelfDisable" runat="server" CssClass="form-control column">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </asp:Panel>

        <%-- =============================================================================== --%>

        <asp:Panel ID="jobPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto" Visible="False">
            <label class="subtitle">Job</label>
            <div class="form-group">
                <label>Date Joined</label>
                <asp:TextBox ID="txtDateJoin" runat="server" CssClass="form-control" placeholder="Date Joined*" type="date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>
                    Department
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Department Required" ControlToValidate="txtDepartment" ForeColor="Red"></asp:RequiredFieldValidator>
                </label>
                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" placeholder="Department* - eg. Sales Department"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>
                    Position
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Position Required" ControlToValidate="txtPosition" ForeColor="Red"></asp:RequiredFieldValidator>
                </label>
                <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" placeholder="Position* - eg. Sales consultant"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Job Type</label>
                <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control">
                    <asp:ListItem>Permenant</asp:ListItem>
                    <asp:ListItem>Contract</asp:ListItem>
                    <asp:ListItem>Part Time</asp:ListItem>
                    <asp:ListItem>Internship</asp:ListItem>
                </asp:DropDownList>
            </div>
        </asp:Panel>

        <%-- =============================================================================== --%>

        <asp:Panel ID="salaryPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto" Visible="False">
            <label class="subtitle">Salary</label>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Salary Effective Date</label>
                    <label class="column">
                        Basic Salary
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Basic Salary Required" ControlToValidate="txtBasicSalary" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtSalaryEffectiveDate" runat="server" CssClass="form-control column" placeholder="NRIC*" Style="margin-right: 2%" type="date"></asp:TextBox>
                    <asp:TextBox ID="txtBasicSalary" runat="server" CssClass="form-control column" placeholder="Basic Salary*" Text="0" TextMode="Number" step="any" min="0"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">
                        Hourly Rate
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Hourly Rate Required" ControlToValidate="txtHourlyRate" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                    <label class="column">
                        Overtime Rate
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Overtime Rate Required" ControlToValidate="txtOvertimeRate" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtHourlyRate" runat="server" CssClass="form-control column" placeholder="Hourly Rate per hour* - eg. 10" Style="margin-right: 2%" Text="0" TextMode="Number" step="any" min="0"></asp:TextBox>
                    <asp:TextBox ID="txtOvertimeRate" runat="server" CssClass="form-control column" placeholder="Overtime Rate per hour* - eg. 10" Text="0" TextMode="Number" step="any" min="0"></asp:TextBox>
                </div>
            </div>
            <label class="subtitle">Payment</label>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">
                        Bank
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Bank Required" ControlToValidate="txtBank" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                    <label class="column">
                        Bank Account Number
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Bank AccNo Required" ControlToValidate="txtBankAccNo" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtBank" runat="server" CssClass="form-control column" placeholder="Bank* - eg. Public Bank" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtBankAccNo" runat="server" CssClass="form-control column" placeholder="Bank Account Number*" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Pay Period</label>
                    <label class="column">Payment Method</label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlPayPeriod" runat="server" CssClass="form-control column" Style="margin-right: 2%">
                        <asp:ListItem>Monthly</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                        <asp:ListItem>Semi-Monthly</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlPayMethod" runat="server" CssClass="form-control column">
                        <asp:ListItem>Bank</asp:ListItem>
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Cheque</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <label class="subtitle">Statutory</label>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Employer EPF Rate</label>
                    <label class="column">Employee EPF Rate</label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlEmployerEpfRate" runat="server" CssClass="form-control column" Style="margin-right: 2%">
                        <asp:ListItem Value="0.00">No Contribution</asp:ListItem>
                        <asp:ListItem Value="0.04">4%</asp:ListItem>
                        <asp:ListItem Value="0.06">6%</asp:ListItem>
                        <asp:ListItem Value="0.065">6.5%</asp:ListItem>
                        <asp:ListItem Value="0.12">12%</asp:ListItem>
                        <asp:ListItem Value="0.13">13%</asp:ListItem>
                        <asp:ListItem Value="0.14">14%</asp:ListItem>
                        <asp:ListItem Value="0.15">15%</asp:ListItem>
                        <asp:ListItem Value="0.16">16%</asp:ListItem>
                        <asp:ListItem Value="0.17">17%</asp:ListItem>
                        <asp:ListItem Value="0.18">18%</asp:ListItem>
                        <asp:ListItem Value="0.19">19%</asp:ListItem>
                        <asp:ListItem Value="0.20">20%</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEmployeeEpfRate" runat="server" CssClass="form-control column">
                        <asp:ListItem Value="0.00">No Contribution</asp:ListItem>
                        <asp:ListItem Value="0.04">4%</asp:ListItem>
                        <asp:ListItem Value="0.055">5.5%</asp:ListItem>
                        <asp:ListItem Value="0.07">7%</asp:ListItem>
                        <asp:ListItem Value="0.08">8%</asp:ListItem>
                        <asp:ListItem Value="0.09">9%</asp:ListItem>
                        <asp:ListItem Value="0.10">10%</asp:ListItem>
                        <asp:ListItem Value="0.11">11%</asp:ListItem>
                        <asp:ListItem Value="0.12">12%</asp:ListItem>
                        <asp:ListItem Value="0.13">13%</asp:ListItem>
                        <asp:ListItem Value="0.14">14%</asp:ListItem>
                        <asp:ListItem Value="0.15">15%</asp:ListItem>
                        <asp:ListItem Value="0.16">16%</asp:ListItem>
                        <asp:ListItem Value="0.17">17%</asp:ListItem>
                        <asp:ListItem Value="0.18">18%</asp:ListItem>
                        <asp:ListItem Value="0.19">19%</asp:ListItem>
                        <asp:ListItem Value="0.20">20%</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label>EPF Membership No.</label>
                <asp:TextBox ID="txtEpfNo" runat="server" CssClass="form-control" placeholder="EPF Membership No. (number only)*" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">SOCSO Category</label>
                    <label class="column">EIS Contribution</label>
                </div>
                <div class="rows">
                    <asp:DropDownList ID="ddlSocsoCategory" runat="server" CssClass="form-control column" Style="margin-right: 2%">
                        <asp:ListItem>No Contribution</asp:ListItem>
                        <asp:ListItem>Employment Injury & Invalidity</asp:ListItem>
                        <asp:ListItem>Employment Injury Only</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEisContribution" runat="server" CssClass="form-control column">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Tax Reference Number</label>
                    <label class="column">Tax Status</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtTaxNo" runat="server" CssClass="form-control column" placeholder="Tax Reference Number* (number only)" Style="margin-right: 2%" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                    <asp:DropDownList ID="ddlTaxStatus" runat="server" CssClass="form-control column">
                        <asp:ListItem>No Contribution</asp:ListItem>
                        <asp:ListItem>Tax Resident</asp:ListItem>
                        <asp:ListItem>Non Tax Resident</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </asp:Panel>


        <footer id="footer" align="center" class="pt-2 fixed-profile-footer" style="border-top: 1px solid #c9c9c9;">
            <asp:Button ID="btnProfile" runat="server" Text="Personal" CssClass="profileMenuButton profileMenuButtonActive" CausesValidation="False" OnClick="btnProfile_Click" OnClientClick="checkAndShow()" />
            <asp:Button ID="btnJob" runat="server" Text="Job" CssClass="profileMenuButton" CausesValidation="False" OnClick="btnJob_Click" />
            <asp:Button ID="btnSalary" runat="server" Text="Salary" CssClass="profileMenuButton" CausesValidation="False" OnClick="btnSalary_Click" />
        </footer>

    </div>

    <script type="text/javascript">

        function showimagepreview(input) {

            if (typeof (input.files) != "undefined") {
                var size = parseFloat(input.files[0].size / 1024).toFixed(2);
                if (size > 5000) {
                    alert("Image has exceeded 5 MB");
                    //var fu = input
                    //if (fu != null) {
                    //    fu.setAttribute("type", "input");
                    //    fu.setAttribute("type", "file");
                    //    document.getElementById("imgProfile").setAttribute("src", "");
                    //}

                }
                else {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {

                            document.getElementById("imgProfile").setAttribute("src", e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);

                    }
                }
                document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";
            }
        }

        function maritalstatuschange(ddlMaritalStatus) {
            if (ddlMaritalStatus.value == "Married") {
                document.getElementById("ddlSpouseWork").disabled = false;
                document.getElementById("txtNoChild").disabled = false;
                document.getElementById("ddlSpouseDisable").disabled = false;
            } else if (ddlMaritalStatus.value == "Single") {
                document.getElementById("ddlSpouseWork").disabled = true;
                document.getElementById("txtNoChild").disabled = true;
                document.getElementById("ddlSpouseDisable").disabled = true;
                document.getElementById("ddlSpouseWork").value = "No";
                document.getElementById("txtNoChild").value = "0";
                document.getElementById("ddlSpouseDisable").value = "No";
            } else {
                document.getElementById("ddlSpouseWork").disabled = true;
                document.getElementById("txtNoChild").disabled = false;
                document.getElementById("ddlSpouseDisable").disabled = true;
                document.getElementById("ddlSpouseWork").value = "No";
                document.getElementById("ddlSpouseDisable").value = "No";
            }
            return false;
        }

        function hideDeleteButton() {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
            return true;
        }

        var x = null;
        x = document.getElementById('<%= btnSave.ClientID %>');
        var y = null;
        y = document.getElementById('<%= btnEditt.ClientID %>');        
        if (x === null || y !== null) {
            document.getElementById("lblFileUpload").style.display = "none";
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else {
            document.getElementById("lblFileUpload").style.display = "block";
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";

        }
        var url = "deleted!)^%$(!";
        url = document.getElementById("imgProfile").getAttribute("src");
        if (url == "data:Image/png;base64,iVBORw0KGgoAAAANSUhEUgAAALQAAAC0CAMAAAAKE/YAAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAAA/UExURUxpcRAQEAwMDCIiIgUFBQYGBhMTEwEBAQcHBwICAj09PQ8PDyIiIhMTEwkJCRISEi0tLTs7O1RUVH5+fgAAADZOelUAAAAUdFJOUwA7F+sF0U79Kr2k+W+L5qbJ+4ztAhpVBgAABoVJREFUeNrtnYtW4yAQhgdCGBJISCvv/6wLaau92QYYJOzpePZ41Kqfs3+AuQAAT0wI4ExVNqYg0vi4DNXNSCZAbCRWZhq024FhZ7d6WfboX78Lc26w8j2xYMHJ4eW7cHVw36BeisQ/f1a73Zm2/BU1H/xftjfmIJIXGjH9boRxhz2b38QxabdH5JUaJ/F0Qplwr8wnav6Eetot8Zn7gVrwvTO7Jwox+2d22ghxM6X0rgHqmf2M1wLU7BowdMOVrMWwfz+fqH9kLRg2wRwE8jM1NuLoG4HIRpBX7PN83sZTeLHV1QJMS8wOpUcWwjpsCdqGGcaroyVNu1k1MYE/6APk4BozH+oe+yIB6Xc4XUAfRxjnP9AhbZz7BV9I6uHwbx6sHcdpGsdl6Vdvk7ocPTSpR1H3k7mOMIQZB4203v6CAyVzN8nHtCGXY6fpsBEPhNAeWa1T7H0GCLgyi3aE0JrKy/OkBDzNBIXPqbEjCvYRNVBF+AsD+DV5Ff4aORKlVYigfcw5vU+B82NHpBAgYZ7Nlty3kMtuoNF1ErZl7NlCMmIDEfNGo6EGAm1sZxaemuBphPxn0ESVoGSXT50N/TwH+8LZJp86Wx6Liq726crQGCHo74rfUlceiKOAeOq5JrQf7RQk2Jip6jxoPYkU6NwRBPIczVKYQVjEep5Oc7R3dV6uBXIcrSWkUfOhFrQfozkkmqmn6THR0X4JMleDNqmOXovwNaD92JGsDpFXas2BXiAdeqwFPWZAH5uE1pWgpxzorg40pkMDHA91hjxtGoTOGKZFLegcedSDzhs9us84vR3atgjd5DSeGGydA65aQUD6mFdxPT2KVHVIXQva9SrV01OtcKvNwBbR8mRJV4MOWTHx9+rITNYkZmtU7ypmmNISkGKqm+p1KaqWVVO9aZEAz+7lya659CzuWQwtdbVrLogLj6EWuRlTGui7Vuy3Q3RfvyS3Uscx5xtFmXkztQCajniSgr5XyMaCPk0XP03rhB63zOeCosRMBf3dWPNmfCbbsUTUDhT6VPi6z/V3Nw9kO5ZooE/ODu01T7DFuu1gIuxyo4Jep3QrH3vcxNqZZzvC30MJ7f2ou0k+7LCSZphpWyApPX3u2xyPRjK+GjPHcdDk+19JoR2eOmSxOyyrdUjfH0sO7Z7JgL6Jmhz62+Vler5LQpe1D/QH+gP9gf5Af6D3CB1/PMrH0wkLOr9+XpZxu1lrQ0iQt1rNqSN64mGSKrLpO8SLfd5urpxCUf8YW20+NKnPiXOTobEzSiQXP0HJjN1ckKiM3nAAyKD23j7gH0KjO9XicpDDN7PU3VyQwtyNHEgscScGpDAbAUTGlj/xNLoDHTMAT+mhgHhmCZQmEqq3EO9noDUev0EAYvV8FEBOXdTTIeVPzpxQPIqDxoVDATOR4zVEMRM/hFcPYyloL46jECWYY88YiZLHokCUcnUMdQy0NlDKVF/G0+jKPIXnjsgimo6sgRfdOAcR8wovCB11RlEE9CQKQoPS9PLw4x2DktQ8Qh/bodN7NLdBLyWgbVF1gIjYmb1d0xOUpY5YgEDEzFISWgDrqKEzOnj/a2jVFZBHi9AzK/wgRjSCf6D/avWxHbrshBja1+mhC8/iYYMz/Sqv5x/o/0MeRR7E4tCfcbrlabxJaDSF5SHpg4Di4RZEJCEBt0LPqmw0vjmwRdx8cGXpvEfEKTYaus3QsyzIHLNxLupc054VGzp4TD3xEHHsLZbKT3vmMaIUEHlWL9oS1IE5qujioWM6GHBgYa+CIKpinH+QWmLqAKi/1it+IqjniXjhJJiJqsith2aLqO2B3iWzHY+rGSNPxoKd7lLjN3a5YO1k8tuMt+PZ7By7/cX6/20W9S2npn99OFt3Z/2Ndc/scGVaY3Q3UzgIXmRtPL9us1s9hugub+780c2r8b5FJ7qhIETYFNtHb+GffO3nq/m/KlxuIBq7KABPS2TVtwR9uRvFNHg1CvC+wUtovKtbgb46sIrbVi5Wsi1eYXW9hhCqCVnjXUZgasHV0+3qUuxf1oiWoNGs3mh3FVpa3LWzh6cLeU65S5razbP9JfgQZt4lNeLLig+b5l1eCdu/biSWO1zyvYlLfXSs7Lyba45XDG3VlgMtrN7HhdKrl+3GtJaQQ7eHK5rDLU0RmTjO3l2S3g9rvD2Ed0N/+mjoScz/7GB2koxHbPEIdyGx+8vhL+mLFyZ/3tLtnEXh4oLyaP8AdQQMq6fJbkwAAAAASUVORK5CYII=") {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else if (url == "deleted!)^%$(!") {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else if (x !== null || y === null) {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";
        }

        function hideUploadFileDeleteButton() {
            document.getElementById("lblFileUpload").style.display = "none";
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
            return true;
        }

        function showUploadFile() {
            document.getElementById("lblFileUpload").style.display = "block";
            return true;
        }

        function checkAndShow() {

            return true;
        }
    </script>

</asp:Content>
