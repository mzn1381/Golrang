//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PCLOR.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public string OrgCode {
            get {
                return ((string)(this["OrgCode"]));
            }
            set {
                this["OrgCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1398")]
        public string Year {
            get {
                return ((string)(this["Year"]));
            }
            set {
                this["Year"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PCLOR_1_1400;Integrated Security=True;")]
        public string PCLOR {
            get {
                return ((string)(this["PCLOR"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PACNT_1_1400;Integrated Security=True;")]
        public string PACNT {
            get {
                return ((string)(this["PACNT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PBANK_1_1400;Integrated Security=True;")]
        public string PBANK {
            get {
                return ((string)(this["PBANK"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PWHRS_1_1400;Integrated Security=True;")]
        public string PWHRS {
            get {
                return ((string)(this["PWHRS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PBASE_1;Integrated Security=True;")]
        public string PBASE {
            get {
                return ((string)(this["PBASE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PSALE_1_1400;Integrated Security=True;")]
        public string PSALE {
            get {
                return ((string)(this["PSALE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=PERP_MAIN;Integrated Security=True;")]
        public string MAIN {
            get {
                return ((string)(this["MAIN"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1398/01/01-1398/12/29")]
        public string date1 {
            get {
                return ((string)(this["date1"]));
            }
            set {
                this["date1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1398/01/01-1398/12/29")]
        public string date2 {
            get {
                return ((string)(this["date2"]));
            }
            set {
                this["date2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\")]
        public string BackupPath {
            get {
                return ((string)(this["BackupPath"]));
            }
            set {
                this["BackupPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ShowPriceAlert {
            get {
                return ((int)(this["ShowPriceAlert"]));
            }
            set {
                this["ShowPriceAlert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RegisterReturnSaleFactorWithGoods {
            get {
                return ((bool)(this["RegisterReturnSaleFactorWithGoods"]));
            }
            set {
                this["RegisterReturnSaleFactorWithGoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ExtraMethod {
            get {
                return ((bool)(this["ExtraMethod"]));
            }
            set {
                this["ExtraMethod"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RegisterSaleFactorWithGoods {
            get {
                return ((bool)(this["RegisterSaleFactorWithGoods"]));
            }
            set {
                this["RegisterSaleFactorWithGoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RegisterSaleFactorNoteGoods {
            get {
                return ((bool)(this["RegisterSaleFactorNoteGoods"]));
            }
            set {
                this["RegisterSaleFactorNoteGoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SalePCBes {
            get {
                return ((bool)(this["SalePCBes"]));
            }
            set {
                this["SalePCBes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SalePCBed {
            get {
                return ((bool)(this["SalePCBed"]));
            }
            set {
                this["SalePCBed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool chk_Net {
            get {
                return ((bool)(this["chk_Net"]));
            }
            set {
                this["chk_Net"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AggregationSaleDoc {
            get {
                return ((bool)(this["AggregationSaleDoc"]));
            }
            set {
                this["AggregationSaleDoc"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaleGoodACCNum {
            get {
                return ((bool)(this["SaleGoodACCNum"]));
            }
            set {
                this["SaleGoodACCNum"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool chk_Total {
            get {
                return ((bool)(this["chk_Total"]));
            }
            set {
                this["chk_Total"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1400/01/01-1400/01/01")]
        public string KardexTedadi {
            get {
                return ((string)(this["KardexTedadi"]));
            }
            set {
                this["KardexTedadi"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool kartexround {
            get {
                return ((bool)(this["kartexround"]));
            }
            set {
                this["kartexround"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1400/01/01-1400/01/01")]
        public string KardexRiyali {
            get {
                return ((string)(this["KardexRiyali"]));
            }
            set {
                this["KardexRiyali"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SalePriceCompute {
            get {
                return ((bool)(this["SalePriceCompute"]));
            }
            set {
                this["SalePriceCompute"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Gift100Darsad {
            get {
                return ((bool)(this["Gift100Darsad"]));
            }
            set {
                this["Gift100Darsad"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool autoSeri {
            get {
                return ((bool)(this["autoSeri"]));
            }
            set {
                this["autoSeri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SanadWithPrice {
            get {
                return ((bool)(this["SanadWithPrice"]));
            }
            set {
                this["SanadWithPrice"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowSaleFactorSentence {
            get {
                return ((bool)(this["ShowSaleFactorSentence"]));
            }
            set {
                this["ShowSaleFactorSentence"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowCustomerBill {
            get {
                return ((bool)(this["ShowCustomerBill"]));
            }
            set {
                this["ShowCustomerBill"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DontShowLogo {
            get {
                return ((bool)(this["DontShowLogo"]));
            }
            set {
                this["DontShowLogo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool giftOrder {
            get {
                return ((bool)(this["giftOrder"]));
            }
            set {
                this["giftOrder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public short SaleFactorStyle {
            get {
                return ((short)(this["SaleFactorStyle"]));
            }
            set {
                this["SaleFactorStyle"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SaveACCRemain {
            get {
                return ((string)(this["SaveACCRemain"]));
            }
            set {
                this["SaveACCRemain"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int printorder {
            get {
                return ((int)(this["printorder"]));
            }
            set {
                this["printorder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public short PrefactorStyle {
            get {
                return ((short)(this["PrefactorStyle"]));
            }
            set {
                this["PrefactorStyle"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public string TypePWHRS {
            get {
                return ((string)(this["TypePWHRS"]));
            }
            set {
                this["TypePWHRS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PrintWithoutLogo {
            get {
                return ((bool)(this["PrintWithoutLogo"]));
            }
            set {
                this["PrintWithoutLogo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EffTime1 {
            get {
                return ((bool)(this["EffTime1"]));
            }
            set {
                this["EffTime1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1398/01/01-1399/12/29")]
        public string Bank_Rec_Report {
            get {
                return ((string)(this["Bank_Rec_Report"]));
            }
            set {
                this["Bank_Rec_Report"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Customer {
            get {
                return ((string)(this["Customer"]));
            }
            set {
                this["Customer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SaleType {
            get {
                return ((string)(this["SaleType"]));
            }
            set {
                this["SaleType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SaleDescription {
            get {
                return ((string)(this["SaleDescription"]));
            }
            set {
                this["SaleDescription"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastUserName {
            get {
                return ((string)(this["LastUserName"]));
            }
            set {
                this["LastUserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BuyFactorPriceCompute {
            get {
                return ((bool)(this["BuyFactorPriceCompute"]));
            }
            set {
                this["BuyFactorPriceCompute"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RegisterBuyFactorWithGoods {
            get {
                return ((bool)(this["RegisterBuyFactorWithGoods"]));
            }
            set {
                this["RegisterBuyFactorWithGoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RegisterBuyFactorNoteGoods {
            get {
                return ((bool)(this["RegisterBuyFactorNoteGoods"]));
            }
            set {
                this["RegisterBuyFactorNoteGoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BuyBaha {
            get {
                return ((bool)(this["BuyBaha"]));
            }
            set {
                this["BuyBaha"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BuyGoodACCNum {
            get {
                return ((bool)(this["BuyGoodACCNum"]));
            }
            set {
                this["BuyGoodACCNum"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PCBes {
            get {
                return ((bool)(this["PCBes"]));
            }
            set {
                this["PCBes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PCBed {
            get {
                return ((bool)(this["PCBed"]));
            }
            set {
                this["PCBed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BuyAggregationSaleDoc {
            get {
                return ((bool)(this["BuyAggregationSaleDoc"]));
            }
            set {
                this["BuyAggregationSaleDoc"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool chk_ByuNet {
            get {
                return ((bool)(this["chk_ByuNet"]));
            }
            set {
                this["chk_ByuNet"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool whtoutgoods {
            get {
                return ((bool)(this["whtoutgoods"]));
            }
            set {
                this["whtoutgoods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public string Returnfactor {
            get {
                return ((string)(this["Returnfactor"]));
            }
            set {
                this["Returnfactor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Returnfactorfunction {
            get {
                return ((string)(this["Returnfactorfunction"]));
            }
            set {
                this["Returnfactorfunction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Ware {
            get {
                return ((string)(this["Ware"]));
            }
            set {
                this["Ware"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string f {
            get {
                return ((string)(this["f"]));
            }
            set {
                this["f"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Print {
            get {
                return ((string)(this["Print"]));
            }
            set {
                this["Print"] = value;
            }
        }
    }
}
