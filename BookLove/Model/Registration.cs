
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
    
public partial class Registration
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Registration()
    {

        this.Order = new HashSet<Order>();

    }


    public int id { get; set; }

    public string firstName { get; set; }

    public string lastName { get; set; }

    public string secondName { get; set; }

    public string numberContract { get; set; }

    public System.DateTime dateOfRegistration { get; set; }

    public int idLogin { get; set; }



    public virtual Login Login { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Order> Order { get; set; }

}

}