//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuildingKirill
{
    using System;
    using System.Collections.Generic;
    
    public partial class ЕдиницыИзмерения
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ЕдиницыИзмерения()
        {
            this.Товар = new HashSet<Товар>();
        }
    
        public int ЕдИзм_ИД { get; set; }
        public string Наименование { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Товар> Товар { get; set; }
    }
}
