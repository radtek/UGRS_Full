SELECT 
  Max([U_InternalFolio]) AS Folio
  FROM [@UG_TR_INTLFRGHT]
  where 
  U_InternalFolio = COALESCE(NULLIF('{Folio}',''), U_InternalFolio) 