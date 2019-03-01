select T0.Code, T0.U_Folio, T0.U_Status, U_Area, U_Employee, U_Date, U_Total, T1.DocTotal from [@UG_GLO_VOUC] T0 with (Nolock)
left join OVPM T1 with (Nolock) on T1.U_GLO_CodeMov = T0.U_Folio and T0.U_Area = T1.U_GLO_CostCenter and DocType = 'A' and U_GLO_PaymentType = 'GLREM'
where  T0.U_TypeVoucher = '0' --and  (T0.U_Total >  ISNULL(T1.DocTotal, 0 ) or (T0.U_Total = 0  and ISNULL(T1.DocTotal, 0 ) = 0))


