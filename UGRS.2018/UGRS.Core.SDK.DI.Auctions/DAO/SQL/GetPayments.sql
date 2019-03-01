select 
   T0.U_GLO_Auxiliar as CardCode, max(T2.CardName) as CardName, sum(Credit) -sum(Debit) as Venta, max(T1.Compra) Compra, 
   max(T3.U_Value) as AccountD, max(T1.AccountC) AccountC 
from JDT1 T0
left join (
       -- Importe de compra
     select 
            C0.U_GLO_Auxiliar, sum(c0.Compra) Compra, max(c0.AccountC) AccountC 
        from ( 
                 select 
                      A0.U_GLO_Auxiliar, sum(A0.Debit- A0.Credit) as Compra, max(A0.Account) AccountC 
                 from JDT1 A0
                 inner join [@UG_CONFIG] A1 on A1.Name =  Case when '{CostingCode}' = 'SU_OBREG' THEN 'SU_COMPRADORSS' ELSE 'SU_COMPRADOR' END
				 and A1.U_Value = A0.Account
                 group by A0.U_GLO_Auxiliar, A0.U_SU_Folio
                 having sum(A0.Debit- A0.Credit)>0 
             ) C0
          group by C0.U_GLO_Auxiliar
    ) T1 
on T1.U_GLO_Auxiliar = T0.U_GLO_Auxiliar
inner join [@UG_CONFIG] T3 on Name = Case when '{CostingCode}' = 'SU_OBREG' THEN 'SU_VENDEDORSS' ELSE 'SU_VENDEDOR' END
 and T3.U_Value = T0.Account
inner join OCRD T2 on T2.CardCode = T0.U_GLO_Auxiliar
where  U_SU_Folio = '{AuctionId}'
group by T0.U_GLO_Auxiliar
having sum(Credit - Debit) > 0





