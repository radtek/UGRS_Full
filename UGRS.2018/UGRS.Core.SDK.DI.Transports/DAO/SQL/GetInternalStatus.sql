  SELECT
  CASE WHEN Count(T0.[U_InternalFolio]) = 1  THEN 'Cerrado' ELSE 
  CASE WHEN Count(T0.[U_InternalFolio]) = 0 THEN 'Abierto' ELSE 'Cancelado' END END AS Stat
  FROM [@UG_TR_INTLFRGHT] T0 with (Nolock)
  INNER JOIN OJDT T1 with (Nolock) ON T0.U_InternalFolio = T1.Ref1
  WHERE T1.TransCode = 'TR/F' AND T0.U_InternalFolio = '{Folio}'