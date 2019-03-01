SELECT T0.U_GLO_Auxiliar AS CardCode, T2.CardName, sum(Credit) - sum(Debit) AS Amount, T3.U_Value AS Account
FROM JDT1 T0
INNER JOIN [@UG_CONFIG] T3 ON Name = Case when '{CostingCode}' = 'SU_OBREG' THEN 'SU_VENDEDORSS' ELSE 'SU_VENDEDOR' END  and T3.U_Value = T0.Account
INNER JOIN OCRD T2 ON T2.CardCode = T0.U_GLO_Auxiliar
WHERE  U_SU_Folio = '{Folio}'
GROUP BY T0.U_GLO_Auxiliar, T2.CardName, U_SU_Folio, T3.U_Value
HAVING (sum(Credit)- sum(Debit)) > 0
ORDER BY CardName ASC