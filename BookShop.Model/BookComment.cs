//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookShop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookComment
    {
        public int Id { get; set; }
        public string Msg { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public int BookId { get; set; }
    }
}
