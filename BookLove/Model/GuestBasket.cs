
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace BookLove.Model
{

using System;
    using System.Collections.Generic;
    
public partial class GuestBasket
{

    public int id { get; set; }

    public string sessionId { get; set; }

    public int idBook { get; set; }

    public int count { get; set; }

    public double totalPrice { get; set; }

    public Nullable<int> idOrder { get; set; }



    public virtual Book Book { get; set; }

    public virtual Order Order { get; set; }

}

}
