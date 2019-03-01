
select count(U_Folio) as U_Folio from [@UG_GLO_VOUC] with (Nolock) where U_Area = '{Area}' and U_TypeVoucher = '{TypeVoucher}' 