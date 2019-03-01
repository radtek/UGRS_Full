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
  FROM [@UG_TR_RODS]
  WHERE U_Origen like '%{Orign}%' and U_Destino like '%{Destiny}%' and U_TR_TOWNORIG like '%{TownOrig}%' and U_TR_TOWNDES like '%{TownDest}%'