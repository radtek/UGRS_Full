SELECT
	   Code
	   ,Name
      ,[U_TypeA]
      ,[U_TypeB]
      ,[U_TypeC]
      ,[U_TypeD]
      ,[U_TypeE]
      ,[U_TypeF]
      ,[U_Destino]
      ,[U_CasetaTC]
      ,[U_CasetaRB]
      ,[U_Activo]
      ,[U_Origen]
      ,[U_TR_TOWNORIG]
      ,[U_TR_TOWNDES]
  FROM [@UG_TR_RODS] with (Nolock)
  WHERE U_Origen like '%{Orign}%' and U_Destino like '%{Destiny}%' and (U_TR_TOWNORIG like '%{TownOrig}%' or U_TR_TOWNORIG is null) and (U_TR_TOWNDES like '%{TownDest}%' or U_TR_TOWNDES is null)