﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;!-- Donation Form   --&gt;
        '''&lt;form id=&quot;donationInputForm&quot;&gt;
        '''    &lt;div class=&quot;donationForm&quot;&gt;
        '''        &lt;div id=&quot;js-processError&quot; class=&quot;p-3 mb-2 bg-danger text-white&quot; style=&quot;display:none;&quot;&gt;&lt;/div&gt;
        '''        &lt;div class=&quot;donationFormInner&quot;&gt;
        '''            &lt;div class=&quot;donateTable&quot;&gt;
        '''                &lt;div class=&quot;form-container&quot;&gt;
        '''                    &lt;div class=&quot;row donate-user donateBox&quot;&gt;
        '''                        &lt;div class=&quot;col-lg-6  donateUser&quot;&gt;
        '''                            &lt;h3&gt;Your Information&lt;/h3&gt;
        '''                    [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property DonationForm() As String
            Get
                Return ResourceManager.GetString("DonationForm", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
