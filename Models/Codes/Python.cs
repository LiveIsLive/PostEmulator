﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 17.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ColdShineSoft.HttpClientPerformer.Models.Codes
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "G:\WindowsApplications\HttpClientPerformer\Models\Codes\Python.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class Python : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\nimport urllib.request\r\nimport urllib.parse\r\n\r\nurl = \'");
            
            #line 10 "G:\WindowsApplications\HttpClientPerformer\Models\Codes\Python.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Task.Url));
            
            #line default
            #line hidden
            this.Write("\'\r\nheaders = {");
            
            #line 11 "G:\WindowsApplications\HttpClientPerformer\Models\Codes\Python.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(",",this.Task.HeaderItems.Where(h => h.Selected).Select(h=>$"'{h.Name}':'{h.Value}'"))));
            
            #line default
            #line hidden
            this.Write("}\r\nrequest = urllib.request.Request(url, method=\'");
            
            #line 12 "G:\WindowsApplications\HttpClientPerformer\Models\Codes\Python.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Task.HttpMethod.Method));
            
            #line default
            #line hidden
            this.Write("\', headers=headers");
            
            #line 12 "G:\WindowsApplications\HttpClientPerformer\Models\Codes\Python.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Task.AcceptsRequestBody?$",data='''{this.Task.PostStringContent}'''":""));
            
            #line default
            #line hidden
            this.Write(")\r\nresponse = urllib.request.urlopen(request)");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
