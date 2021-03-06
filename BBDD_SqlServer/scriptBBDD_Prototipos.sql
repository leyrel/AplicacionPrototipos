USE [master]
GO
/****** Object:  Database [Prototipos]    Script Date: 06/08/2017 10:25:35 ******/
CREATE DATABASE [Prototipos] ON  PRIMARY 
( NAME = N'Prototipos', FILENAME = N'F:\Prototipos.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Prototipos_log', FILENAME = N'G:\Prototipos.ldf' , SIZE = 22528KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Prototipos] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Prototipos].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [Prototipos] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Prototipos] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Prototipos] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Prototipos] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Prototipos] SET ARITHABORT OFF
GO
ALTER DATABASE [Prototipos] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Prototipos] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Prototipos] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Prototipos] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Prototipos] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Prototipos] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Prototipos] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Prototipos] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Prototipos] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Prototipos] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Prototipos] SET  DISABLE_BROKER
GO
ALTER DATABASE [Prototipos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Prototipos] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Prototipos] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Prototipos] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Prototipos] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Prototipos] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Prototipos] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Prototipos] SET  READ_WRITE
GO
ALTER DATABASE [Prototipos] SET RECOVERY FULL
GO
ALTER DATABASE [Prototipos] SET  MULTI_USER
GO
ALTER DATABASE [Prototipos] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Prototipos] SET DB_CHAINING OFF
GO
USE [Prototipos]
GO
/****** Object:  Table [dbo].[tPruebaArchivo]    Script Date: 06/08/2017 10:25:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tPruebaArchivo](
	[IdPrueba] [int] NOT NULL,
	[IdArch] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Tipo] [varchar](256) NULL,
	[Lenght] [money] NULL,
	[Contenido] [varbinary](max) NULL,
	[FechaAdd] [datetime] NULL,
	[UsuarioAdd] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_tPruebaArchivo] PRIMARY KEY CLUSTERED 
(
	[IdPrueba] ASC,
	[IdArch] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneArchivosPrueba]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneArchivosPrueba] (@idPrueba int)

Returns varchar(2)

Begin
declare @tieneArchivos varchar(2)

if exists (select idArch from tPruebaArchivo where idPrueba=@idPrueba) set @tieneArchivos='Sí' else set @tieneArchivos='No'

return @tieneArchivos
END
GO
/****** Object:  Table [dbo].[tFaseArchivo]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tFaseArchivo](
	[IdFase] [int] NOT NULL,
	[IdArch] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Tipo] [varchar](256) NULL,
	[Lenght] [money] NULL,
	[Contenido] [varbinary](max) NULL,
	[FechaAdd] [datetime] NULL,
	[UsuarioAdd] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_tFaseArchivo] PRIMARY KEY CLUSTERED 
(
	[IdFase] ASC,
	[IdArch] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneArchivosFase]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneArchivosFase] (@idFase int)

Returns varchar(2)

Begin
declare @tieneArchivos varchar(2)

if exists (select idArch from tFaseArchivo where idFase=@idFase) set @tieneArchivos='Sí' else set @tieneArchivos='No'

return @tieneArchivos
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneOtrosBloqueosGrua]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneOtrosBloqueosGrua] (@idPrototipo int)

Returns varchar(2)

Begin
declare @tieneBloq varchar(2)

if exists(select idFase from tFase where idPrototipo=@idPrototipo and BloqueoGrua='1'
		  union
		  select idPrueba from tPrueba where idPrototipo=@idPrototipo and BloqueoGrua='1')
set @tieneBloq='Sí' else set @tieneBloq='No'

return @tieneBloq
END
GO
/****** Object:  Table [dbo].[tDeficienciaPruArchivo]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tDeficienciaPruArchivo](
	[IdDeficiencia] [int] NOT NULL,
	[IdArch] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Tipo] [varchar](256) NULL,
	[Lenght] [money] NULL,
	[Contenido] [varbinary](max) NULL,
	[FechaAdd] [datetime] NULL,
	[UsuarioAdd] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_tDeficienciaPruArchivo] PRIMARY KEY CLUSTERED 
(
	[IdDeficiencia] ASC,
	[IdArch] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneArchivosDefPru]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneArchivosDefPru] (@idDeficiencia int)

Returns varchar(2)

Begin
declare @tieneArchivos varchar(2)

if exists (select idArch from tDeficienciaPruArchivo where idDeficiencia=@idDeficiencia) set @tieneArchivos='Sí' else set @tieneArchivos='No'

return @tieneArchivos
END
GO
/****** Object:  Table [dbo].[tDeficienciaArchivo]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tDeficienciaArchivo](
	[IdDeficiencia] [int] NOT NULL,
	[IdArch] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Tipo] [varchar](256) NULL,
	[Lenght] [money] NULL,
	[Contenido] [varbinary](max) NULL,
	[FechaAdd] [datetime] NULL,
	[UsuarioAdd] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_tDeficienciaArchivo] PRIMARY KEY CLUSTERED 
(
	[IdDeficiencia] ASC,
	[IdArch] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneArchivosDef]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneArchivosDef] (@idDeficiencia int)

Returns varchar(2)

Begin
declare @tieneArchivos varchar(2)

if exists (select idArch from tDeficienciaArchivo where idDeficiencia=@idDeficiencia) set @tieneArchivos='Sí' else set @tieneArchivos='No'

return @tieneArchivos
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_tieneDefsConBloqueo]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_tieneDefsConBloqueo] (@idPrototipo int)

Returns varchar(2)

Begin
declare @tieneDefs varchar(2)

if exists(select idDeficiencia from tDeficiencia where idPrototipo=@idPrototipo and Bloqueo='1'
		  union
		  select idDeficiencia from tDeficienciaPrueba where idPrototipo=@idPrototipo and Bloqueo='1')
set @tieneDefs='Sí' else set @tieneDefs='No'

return @tieneDefs
END
GO
/****** Object:  Table [dbo].[tTratamientoPieza]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tTratamientoPieza](
	[IdPieza] [int] IDENTITY(1,1) NOT NULL,
	[IdFase] [int] NOT NULL,
	[Articulo] [varchar](100) NOT NULL,
	[Accion] [varchar](2500) NOT NULL,
 CONSTRAINT [PK_tTratamientoPieza] PRIMARY KEY CLUSTERED 
(
	[IdPieza] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tUsuario]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tUsuario](
	[IdUsuario] [varchar](50) NOT NULL,
	[Usuario] [varchar](50) NOT NULL,
	[Departamento] [varchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
 CONSTRAINT [PK_tUsuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tPrueba]    Script Date: 06/08/2017 10:25:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tPrueba](
	[IdPrototipo] [int] NOT NULL,
	[IdPrueba] [int] IDENTITY(1,1) NOT NULL,
	[FechaPrueba] [datetime] NULL,
	[FechaRegistroPrueba] [datetime] NOT NULL,
	[IdUsuario] [varchar](50) NOT NULL,
	[Prueba] [text] NOT NULL,
	[Deficiencia] [bit] NOT NULL,
	[BloqueoGrua] [bit] NOT NULL,
	[IdUsuarioBloqueoGrua] [varchar](50) NULL,
	[SituacionBloqueoGrua] [text] NULL,
	[FechaDesbloqueoGrua] [datetime] NULL,
	[FechaRegistroDesbloqueoGrua] [datetime] NULL,
	[IdUsuarioDesbloqueoGrua] [varchar](50) NULL,
	[RazonDesbloqueoGrua] [text] NULL,
 CONSTRAINT [PK_tPrueba] PRIMARY KEY CLUSTERED 
(
	[IdPrueba] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_pruebaUsuario]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_pruebaUsuario]
AS
SELECT TOP (99.99) PERCENT dbo.tPrueba.IdPrototipo, dbo.tPrueba.IdPrueba, CONVERT(VarChar, dbo.tPrueba.FechaPrueba, 103) AS FechaPrueba, 
dbo.tPrueba.FechaRegistroPrueba, dbo.tUsuario.Usuario, dbo.tPrueba.Prueba, 
CASE dbo.tPrueba.Deficiencia WHEN 1 THEN 'Sí' ELSE 'No' END AS Deficiencia, dbo.f_tieneArchivosPrueba(dbo.tPrueba.IdPrueba) AS tieneArchivos,
CASE dbo.tPrueba.BloqueoGrua WHEN 1 THEN 'Sí' ELSE 'No' END AS BloqueoGrua, usBloq.Usuario AS UsuarioBloqueoGrua
FROM dbo.tPrueba
INNER JOIN dbo.tUsuario ON dbo.tPrueba.IdUsuario = dbo.tUsuario.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS usBloq ON dbo.tPrueba.IdUsuarioBloqueoGrua = usBloq.IdUsuario
ORDER BY dbo.tPrueba.FechaPrueba desc
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario"
            Begin Extent = 
               Top = 6
               Left = 265
               Bottom = 99
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaUsuario'
GO
/****** Object:  View [dbo].[v_pruebaUsSemana]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_pruebaUsSemana]
AS
SELECT TOP (99.99) PERCENT dbo.tPrueba.IdPrototipo, dbo.tPrueba.IdPrueba, CONVERT(VarChar, dbo.tPrueba.FechaPrueba, 103) AS FechaPrueba, 
dbo.tPrueba.FechaRegistroPrueba, dbo.tUsuario.Usuario, dbo.tPrueba.Prueba, 
CASE dbo.tPrueba.Deficiencia WHEN 1 THEN 'Sí' ELSE 'No' END AS Deficiencia,
CASE dbo.tPrueba.BloqueoGrua WHEN 1 THEN 'Sí' ELSE 'No' END AS BloqueoGrua, usBloq.Usuario AS UsuarioBloqueoGrua
FROM dbo.tPrueba
INNER JOIN dbo.tUsuario ON dbo.tPrueba.IdUsuario = dbo.tUsuario.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS usBloq ON dbo.tPrueba.IdUsuarioBloqueoGrua = usBloq.IdUsuario
WHERE (dbo.tPrueba.FechaPrueba > GETDATE() - 8)
ORDER BY dbo.tPrueba.FechaPrueba
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario"
            Begin Extent = 
               Top = 6
               Left = 265
               Bottom = 99
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaUsSemana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaUsSemana'
GO
/****** Object:  Table [dbo].[tFase]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tFase](
	[IdPrototipo] [int] NOT NULL,
	[IdFase] [int] IDENTITY(1,1) NOT NULL,
	[EsInicial] [bit] NOT NULL,
	[FechaInsercion] [datetime] NOT NULL,
	[FechaPuestaMarcha] [datetime] NULL,
	[IdUsuario] [varchar](50) NOT NULL,
	[ConfiguracionMontaje] [varchar](500) NULL,
	[Observaciones] [text] NULL,
	[Deficiencia] [bit] NOT NULL,
	[DesmontajeFinal] [bit] NULL,
	[Desmontaje] [bit] NULL,
	[FechaDesmontaje] [datetime] NULL,
	[IdUsuarioDesmontaje] [varchar](50) NULL,
	[ObservacionesDesmontaje] [text] NULL,
	[ObservacionesTratamiento] [text] NULL,
	[BloqueoGrua] [bit] NOT NULL,
	[IdUsuarioBloqueoGrua] [varchar](50) NULL,
	[SituacionBloqueoGrua] [text] NULL,
	[FechaDesbloqueoGrua] [datetime] NULL,
	[FechaRegistroDesbloqueoGrua] [datetime] NULL,
	[IdUsuarioDesbloqueoGrua] [varchar](50) NULL,
	[RazonDesbloqueoGrua] [text] NULL,
 CONSTRAINT [PK_tFase] PRIMARY KEY CLUSTERED 
(
	[IdFase] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_faseUsuario]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_faseUsuario]
AS
SELECT TOP (99.99) PERCENT dbo.tFase.IdPrototipo, dbo.tFase.IdFase, CASE dbo.tFase.EsInicial WHEN 1 THEN 'Inicial' ELSE 'Derivada' END AS EsInicial, 
dbo.tFase.FechaInsercion, CONVERT(VarChar, dbo.tFase.FechaPuestaMarcha, 103) AS FechaPuestaMarcha, dbo.tUsuario.Usuario, 
dbo.tFase.ConfiguracionMontaje, dbo.tFase.Observaciones, CASE dbo.tFase.Deficiencia WHEN 1 THEN 'Sí' ELSE 'No' END AS Deficiencia, 
dbo.tFase.DesmontajeFinal, dbo.tFase.Desmontaje, CONVERT(VarChar, dbo.tFase.FechaDesmontaje, 103) AS FechaDesmontaje, 
dbo.tFase.ObservacionesDesmontaje, usDes.Usuario AS UsuarioDesmontaje, dbo.tFase.ObservacionesTratamiento,
dbo.f_tieneArchivosFase(dbo.tFase.IdFase) AS tieneArchivos,
CASE dbo.tFase.BloqueoGrua WHEN 1 THEN 'Sí' ELSE 'No' END AS BloqueoGrua, usBloq.Usuario AS UsuarioBloqueoGrua
FROM dbo.tFase
LEFT OUTER JOIN dbo.tUsuario ON dbo.tFase.IdUsuario = dbo.tUsuario.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS usDes ON dbo.tFase.IdUsuarioDesmontaje = usDes.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS usBloq ON dbo.tFase.IdUsuarioBloqueoGrua = usBloq.IdUsuario
ORDER BY dbo.tFase.FechaPuestaMarcha
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tFase"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 99
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "usDes"
            Begin Extent = 
               Top = 6
               Left = 512
               Bottom = 99
               Right = 701
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseUsuario'
GO
/****** Object:  UserDefinedFunction [dbo].[f_emailsUsuarios]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_emailsUsuarios] ()

Returns varchar(2500)

Begin
declare @signo varchar(1)
declare @email varchar(2500)
declare @emails varchar(2500)
declare @contador int

Set @emails = ''
set @contador=0

DECLARE Emails CURSOR FOR
select distinct Email
from tUsuario

OPEN Emails
FETCH NEXT from Emails INTO  @email

	WHILE @@FETCH_STATUS = 0 
begin
set @contador= @contador + 1
set @signo = '; '
if @contador = 1 
	begin
		set @signo = ' '
	end
	Set @emails = @email + @signo + @emails

	FETCH NEXT FROM Emails
	INTO @email
End
CLOSE Emails
DEALLOCATE Emails

return @emails

END
GO
/****** Object:  Table [dbo].[tDeficiencia]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tDeficiencia](
	[IdPrototipo] [int] NOT NULL,
	[IdFase] [int] NOT NULL,
	[IdDeficiencia] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[IdUsuarioCreador] [varchar](50) NOT NULL,
	[Descripcion] [text] NOT NULL,
	[Limitaciones] [text] NULL,
	[Bloqueo] [bit] NOT NULL,
	[FechaDesbloqueo] [datetime] NULL,
	[FechaRegistroDesbloqueo] [datetime] NULL,
	[IdUsuarioDesbloqueo] [varchar](50) NULL,
	[IdUsuarioBloqueo] [varchar](50) NULL,
	[RazonDesbloqueo] [text] NULL,
	[FechaResolucion] [datetime] NULL,
	[FechaRegistroResolucion] [datetime] NULL,
	[IdUsuarioResolucion] [varchar](50) NULL,
	[ObservacionesResolucion] [text] NULL,
 CONSTRAINT [PK_tDeficiencia] PRIMARY KEY CLUSTERED 
(
	[IdDeficiencia] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_defUsuario]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_defUsuario]
AS
SELECT TOP (99.99) PERCENT dbo.tDeficiencia.IdPrototipo, dbo.tDeficiencia.IdFase, dbo.tDeficiencia.IdDeficiencia, 
dbo.tDeficiencia.Fecha, CONVERT(VarChar, dbo.tDeficiencia.Fecha, 103) AS FechaCorta,
dbo.tUsuario.Usuario AS UsuarioCreador, dbo.tDeficiencia.Descripcion, dbo.tDeficiencia.Limitaciones, 
CASE dbo.tDeficiencia.Bloqueo WHEN 1 THEN 'Sí' ELSE 'No' END AS Bloqueo, tUsuario_1.Usuario AS UsuarioBloqueo,
CONVERT(VarChar, dbo.tDeficiencia.FechaDesbloqueo, 103) AS FechaDesbloqueo, dbo.tDeficiencia.FechaRegistroDesbloqueo,
tUsuario_3.Usuario AS UsuarioDesbloqueo, dbo.tDeficiencia.RazonDesbloqueo,
CASE WHEN dbo.tDeficiencia.FechaResolucion is NULL THEN 'No' ELSE 'Sí' END AS Resuelta,
CONVERT(VarChar, dbo.tDeficiencia.FechaResolucion, 103) AS FechaResolucion,
dbo.tDeficiencia.FechaRegistroResolucion, tUsuario_2.Usuario AS UsuarioResolucion, dbo.tDeficiencia.ObservacionesResolucion,
dbo.f_tieneArchivosDef(dbo.tDeficiencia.IdDeficiencia) AS tieneArchivos
FROM dbo.tDeficiencia
INNER JOIN dbo.tUsuario ON dbo.tDeficiencia.IdUsuarioCreador = dbo.tUsuario.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_3 ON dbo.tDeficiencia.IdUsuarioDesbloqueo = tUsuario_3.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_1 ON dbo.tDeficiencia.IdUsuarioBloqueo = tUsuario_1.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_2 ON dbo.tDeficiencia.IdUsuarioResolucion = tUsuario_2.IdUsuario
ORDER BY dbo.tDeficiencia.Fecha
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficiencia"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 99
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_3"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 99
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_1"
            Begin Extent = 
               Top = 6
               Left = 735
               Bottom = 99
               Right = 924
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_2"
            Begin Extent = 
               Top = 6
               Left = 962
               Bottom = 99
               Right = 1151
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         O' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'r = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defUsuario'
GO
/****** Object:  Table [dbo].[tDeficienciaPrueba]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tDeficienciaPrueba](
	[IdPrototipo] [int] NOT NULL,
	[IdPrueba] [int] NOT NULL,
	[IdDeficiencia] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[IdUsuarioCreador] [varchar](50) NOT NULL,
	[Descripcion] [text] NOT NULL,
	[Limitaciones] [text] NULL,
	[Bloqueo] [bit] NOT NULL,
	[FechaDesbloqueo] [datetime] NULL,
	[FechaRegistroDesbloqueo] [datetime] NULL,
	[IdUsuarioDesbloqueo] [varchar](50) NULL,
	[IdUsuarioBloqueo] [varchar](50) NULL,
	[RazonDesbloqueo] [text] NULL,
	[FechaResolucion] [datetime] NULL,
	[FechaRegistroResolucion] [datetime] NULL,
	[IdUsuarioResolucion] [varchar](50) NULL,
	[ObservacionesResolucion] [text] NULL,
 CONSTRAINT [PK_tDeficienciaPrueba] PRIMARY KEY CLUSTERED 
(
	[IdDeficiencia] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_defPruebaUsuario]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_defPruebaUsuario]
AS
SELECT TOP (99.99) PERCENT dbo.tDeficienciaPrueba.IdPrototipo, dbo.tDeficienciaPrueba.IdPrueba, dbo.tDeficienciaPrueba.IdDeficiencia,
dbo.tDeficienciaPrueba.Fecha, CONVERT(VarChar, dbo.tDeficienciaPrueba.Fecha, 103) AS FechaCorta,
dbo.tUsuario.Usuario AS UsuarioCreador, dbo.tDeficienciaPrueba.Descripcion, dbo.tDeficienciaPrueba.Limitaciones, 
CASE dbo.tDeficienciaPrueba.Bloqueo WHEN 1 THEN 'Sí' ELSE 'No' END AS Bloqueo, tUsuario_1.Usuario AS UsuarioBloqueo,
CONVERT(VarChar, dbo.tDeficienciaPrueba.FechaDesbloqueo, 103) AS FechaDesbloqueo, dbo.tDeficienciaPrueba.FechaRegistroDesbloqueo, 
tUsuario_3.Usuario AS UsuarioDesbloqueo, dbo.tDeficienciaPrueba.RazonDesbloqueo,
CASE WHEN dbo.tDeficienciaPrueba.FechaResolucion is NULL THEN 'No' ELSE 'Sí' END AS Resuelta,
CONVERT(VarChar, dbo.tDeficienciaPrueba.FechaResolucion, 103) AS FechaResolucion, dbo.tDeficienciaPrueba.FechaRegistroResolucion,
tUsuario_2.Usuario AS UsuarioResolucion, dbo.tDeficienciaPrueba.ObservacionesResolucion,
dbo.f_tieneArchivosDefPru(dbo.tDeficienciaPrueba.IdDeficiencia) AS tieneArchivos
FROM dbo.tDeficienciaPrueba
INNER JOIN dbo.tUsuario ON dbo.tDeficienciaPrueba.IdUsuarioCreador = dbo.tUsuario.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_3 ON dbo.tDeficienciaPrueba.IdUsuarioDesbloqueo = tUsuario_3.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_1 ON dbo.tDeficienciaPrueba.IdUsuarioBloqueo = tUsuario_1.IdUsuario
LEFT OUTER JOIN dbo.tUsuario AS tUsuario_2 ON dbo.tDeficienciaPrueba.IdUsuarioResolucion = tUsuario_2.IdUsuario
ORDER BY dbo.tDeficienciaPrueba.Fecha
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficienciaPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 99
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_3"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 99
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_1"
            Begin Extent = 
               Top = 6
               Left = 735
               Bottom = 99
               Right = 924
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tUsuario_2"
            Begin Extent = 
               Top = 6
               Left = 962
               Bottom = 99
               Right = 1151
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defPruebaUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defPruebaUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_defPruebaUsuario'
GO
/****** Object:  Table [dbo].[tSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tSistema](
	[IdSistema] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Orden] [int] NULL,
 CONSTRAINT [PK_tSistema] PRIMARY KEY CLUSTERED 
(
	[IdSistema] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tPruebaAfecta]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tPruebaAfecta](
	[IdDeficienciaPru] [int] NOT NULL,
	[IdSistema] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[Otros] [varchar](100) NULL,
 CONSTRAINT [PK_tPruebaAfecta] PRIMARY KEY CLUSTERED 
(
	[IdDeficienciaPru] ASC,
	[IdSistema] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_pruebaSinResSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_pruebaSinResSistema]
AS
SELECT DISTINCT 
                      TOP (100) PERCENT dbo.tDeficienciaPrueba.IdPrototipo, dbo.tPruebaAfecta.IdSistema, dbo.tSistema.Nombre, dbo.tPruebaAfecta.Otros, 
                      dbo.tSistema.Orden
FROM         dbo.tDeficienciaPrueba INNER JOIN
                      dbo.tPruebaAfecta ON dbo.tDeficienciaPrueba.IdDeficiencia = dbo.tPruebaAfecta.IdDeficienciaPru INNER JOIN
                      dbo.tSistema ON dbo.tPruebaAfecta.IdSistema = dbo.tSistema.IdSistema
WHERE     (dbo.tDeficienciaPrueba.FechaResolucion IS NULL)
ORDER BY dbo.tDeficienciaPrueba.IdPrototipo, dbo.tSistema.Orden
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficienciaPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tPruebaAfecta"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 114
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 99
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaSinResSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaSinResSistema'
GO
/****** Object:  View [dbo].[v_pruebaDefSinResSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_pruebaDefSinResSistema]
AS
SELECT     dbo.tDeficienciaPrueba.IdPrototipo, dbo.tDeficienciaPrueba.IdPrueba, dbo.tPruebaAfecta.IdDeficienciaPru, dbo.tPruebaAfecta.IdSistema, 
                      dbo.tPruebaAfecta.Fecha, dbo.tSistema.Nombre, dbo.tPruebaAfecta.Otros, dbo.tSistema.Orden
FROM         dbo.tDeficienciaPrueba INNER JOIN
                      dbo.tPruebaAfecta ON dbo.tDeficienciaPrueba.IdDeficiencia = dbo.tPruebaAfecta.IdDeficienciaPru INNER JOIN
                      dbo.tSistema ON dbo.tPruebaAfecta.IdSistema = dbo.tSistema.IdSistema
WHERE     (dbo.tDeficienciaPrueba.FechaResolucion IS NULL)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficienciaPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 137
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tPruebaAfecta"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 115
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 106
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaDefSinResSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaDefSinResSistema'
GO
/****** Object:  View [dbo].[v_pruebaAfectaSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_pruebaAfectaSistema]
AS
SELECT     dbo.tDeficienciaPrueba.IdPrototipo, dbo.tDeficienciaPrueba.IdPrueba, dbo.tPruebaAfecta.IdDeficienciaPru, dbo.tPruebaAfecta.IdSistema, 
                      dbo.tSistema.Nombre, dbo.tPruebaAfecta.Otros, dbo.tSistema.Orden
FROM         dbo.tDeficienciaPrueba INNER JOIN
                      dbo.tPruebaAfecta ON dbo.tDeficienciaPrueba.IdDeficiencia = dbo.tPruebaAfecta.IdDeficienciaPru INNER JOIN
                      dbo.tSistema ON dbo.tPruebaAfecta.IdSistema = dbo.tSistema.IdSistema
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficienciaPrueba"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tPruebaAfecta"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 114
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 99
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaAfectaSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_pruebaAfectaSistema'
GO
/****** Object:  Table [dbo].[tFaseAfecta]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tFaseAfecta](
	[IdDeficienciaFas] [int] NOT NULL,
	[IdSistema] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[Otros] [varchar](100) NULL,
 CONSTRAINT [PK_tFaseAfecta] PRIMARY KEY CLUSTERED 
(
	[IdDeficienciaFas] ASC,
	[IdSistema] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[v_faseSinResSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_faseSinResSistema]
AS
SELECT DISTINCT 
                      TOP (100) PERCENT dbo.tDeficiencia.IdPrototipo, dbo.tFaseAfecta.IdSistema, dbo.tSistema.Nombre, dbo.tFaseAfecta.Otros, dbo.tSistema.Orden
FROM         dbo.tFaseAfecta INNER JOIN
                      dbo.tSistema ON dbo.tFaseAfecta.IdSistema = dbo.tSistema.IdSistema INNER JOIN
                      dbo.tDeficiencia ON dbo.tFaseAfecta.IdDeficienciaFas = dbo.tDeficiencia.IdDeficiencia
WHERE     (dbo.tDeficiencia.FechaResolucion IS NULL)
ORDER BY dbo.tDeficiencia.IdPrototipo, dbo.tSistema.Orden
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tFaseAfecta"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 6
               Left = 265
               Bottom = 97
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tDeficiencia"
            Begin Extent = 
               Top = 6
               Left = 492
               Bottom = 128
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseSinResSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseSinResSistema'
GO
/****** Object:  View [dbo].[v_faseDefSinResSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_faseDefSinResSistema]
AS
SELECT     dbo.tDeficiencia.IdPrototipo, dbo.tDeficiencia.IdFase, dbo.tFaseAfecta.IdDeficienciaFas, dbo.tFaseAfecta.IdSistema, dbo.tFaseAfecta.Fecha, 
                      dbo.tSistema.Nombre, dbo.tFaseAfecta.Otros, dbo.tSistema.Orden
FROM         dbo.tFaseAfecta INNER JOIN
                      dbo.tSistema ON dbo.tFaseAfecta.IdSistema = dbo.tSistema.IdSistema INNER JOIN
                      dbo.tDeficiencia ON dbo.tFaseAfecta.IdDeficienciaFas = dbo.tDeficiencia.IdDeficiencia
WHERE     (dbo.tDeficiencia.FechaResolucion IS NULL)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tFaseAfecta"
            Begin Extent = 
               Top = 43
               Left = 302
               Bottom = 152
               Right = 491
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 29
               Left = 567
               Bottom = 133
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tDeficiencia"
            Begin Extent = 
               Top = 43
               Left = 38
               Bottom = 151
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseDefSinResSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseDefSinResSistema'
GO
/****** Object:  View [dbo].[v_faseAfectaSistema]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_faseAfectaSistema]
AS
SELECT     dbo.tDeficiencia.IdPrototipo, dbo.tDeficiencia.IdFase, dbo.tFaseAfecta.IdDeficienciaFas, dbo.tFaseAfecta.IdSistema, dbo.tSistema.Nombre, 
                      dbo.tFaseAfecta.Otros, dbo.tSistema.Orden
FROM         dbo.tDeficiencia INNER JOIN
                      dbo.tFaseAfecta ON dbo.tDeficiencia.IdDeficiencia = dbo.tFaseAfecta.IdDeficienciaFas INNER JOIN
                      dbo.tSistema ON dbo.tFaseAfecta.IdSistema = dbo.tSistema.IdSistema
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tDeficiencia"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tFaseAfecta"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 114
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tSistema"
            Begin Extent = 
               Top = 6
               Left = 508
               Bottom = 99
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseAfectaSistema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_faseAfectaSistema'
GO
/****** Object:  Table [dbo].[tPrototipo]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tPrototipo](
	[IdPrototipo] [int] IDENTITY(1,1) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[IdUsuarioCreador] [varchar](50) NOT NULL,
	[Proyecto] [varchar](250) NOT NULL,
	[Responsable] [varchar](50) NOT NULL,
	[Configuracion] [varchar](500) NOT NULL,
	[Descripcion] [text] NULL,
	[Observaciones] [text] NULL,
	[Estado] [int] NOT NULL,
	[FechaCierre] [datetime] NULL,
	[IdUsuarioCierre] [varchar](50) NULL,
	[Cierre] [text] NULL,
 CONSTRAINT [PK_tPrototipo] PRIMARY KEY CLUSTERED 
(
	[IdPrototipo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[pa_prot_MensajePeriodico_BloqueoGruaPrueba]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pa_prot_MensajePeriodico_BloqueoGruaPrueba] AS
DECLARE @Proyecto varchar(250)
DECLARE @Prueba varchar(500)
DECLARE @Email varchar(50)
DECLARE @SituacionBloqueoGrua varchar(max)

Declare @PruebasConBloqueo
Table(rowid int identity(1,1),
	  Proyecto VARCHAR(250),
	  Prueba VARCHAR(500),
	  Email VARCHAR(50),
	  SituacionBloqueoGrua VARCHAR(max));

Insert Into @PruebasConBloqueo
Select p.Proyecto, pru.Prueba, u.Email, pru.SituacionBloqueoGrua
from tPrueba pru
join tPrototipo p on pru.IdPrototipo = p.IdPrototipo
join tUsuario u on pru.IdusuarioBloqueoGrua = u.IdUsuario
where pru.BloqueoGrua=1

Declare @i as Int;
Select @i = max(rowid) from @PruebasConBloqueo

While @i > 0
BEGIN
DECLARE @asunto varchar(100)
DECLARE @msg varchar(max)

Select @Proyecto = Proyecto from @PruebasConBloqueo where rowid = @i 
Select @Prueba = Prueba from @PruebasConBloqueo where rowid = @i
select @Email =  Email FROM @PruebasConBloqueo  where rowid = @i
select @SituacionBloqueoGrua =  SituacionBloqueoGrua FROM @PruebasConBloqueo  where rowid = @i

set @asunto = 'Existe un BLOQUEO para la grúa LC2064'
SET @msg = 'Hola, <br/><br/>Le informamos de que creó un bloqueo o limitación de uso para la grúa LC2064 con una prueba del <b>proyecto</b>: '+ @Proyecto +
'<br/><br/><span style="color: #1F497D;"><b>Prueba:</b></span>' +
'<br/><b>&nbsp;&nbsp;&nbsp;Actividad realizada</b>: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + @Prueba +
'<br/><br/><span style="color: #1F497D;"><b>Situación del bloqueo</b></span>: &nbsp;&nbsp;' + @SituacionBloqueoGrua +
'<br/><br/>Por favor, considere si hay que mantenerlo o quitarlo.
<br/><br/>Saludos,'

EXEC msdb.dbo.sp_send_dbmail	@profile_name = 'AlertasAplicacion_Prototipos',
								@importance = 'High',
								@recipients = @Email,
								@subject = @asunto,
								@body_format = 'HTML',
								@body = @msg
Set @i = @i -1
END
GO
/****** Object:  StoredProcedure [dbo].[pa_prot_MensajePeriodico_BloqueoGrua]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pa_prot_MensajePeriodico_BloqueoGrua] AS
DECLARE @Proyecto varchar(250)
DECLARE @ConfiguracionMontaje varchar(500)
DECLARE @Inicial varchar(8)
DECLARE @Email varchar(50)
DECLARE @SituacionBloqueoGrua varchar(max)

Declare @FasesConBloqueo
Table(rowid int identity(1,1),
	  Proyecto VARCHAR(250),
	  ConfiguracionMontaje VARCHAR(500),
	  Inicial VARCHAR(8),
	  Email VARCHAR(50),
	  SituacionBloqueoGrua VARCHAR(max));

Insert Into @FasesConBloqueo
Select p.Proyecto, f.ConfiguracionMontaje, CASE f.EsInicial WHEN 1 THEN 'Inicial' ELSE 'Derivada' END,
u.Email, f.SituacionBloqueoGrua
from tFase f
join tPrototipo p on f.IdPrototipo = p.IdPrototipo
join tUsuario u on f.IdusuarioBloqueoGrua = u.IdUsuario
where f.BloqueoGrua=1

Declare @i as Int;
Select @i = max(rowid) from @FasesConBloqueo

While @i > 0
BEGIN
DECLARE @asunto varchar(100)
DECLARE @msg varchar(max)

Select @Proyecto = Proyecto from @FasesConBloqueo where rowid = @i 
Select @ConfiguracionMontaje = ConfiguracionMontaje from @FasesConBloqueo where rowid = @i
select @Inicial =  Inicial FROM @FasesConBloqueo where rowid = @i
select @Email =  Email FROM @FasesConBloqueo  where rowid = @i
select @SituacionBloqueoGrua =  SituacionBloqueoGrua FROM @FasesConBloqueo  where rowid = @i

set @asunto = 'Existe un BLOQUEO para la grúa LC2064'
SET @msg = 'Hola, <br/><br/>Le informamos de que creó un bloqueo o limitación de uso para la grúa LC2064 con una puesta en marcha del <b>proyecto</b>: '+ @Proyecto +
'<br/><br/><span style="color: #1F497D;"><b>Puesta en marcha:</b></span>' +
'<br/><b>&nbsp;&nbsp;&nbsp;Tipo</b>: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + @Inicial +
'<br/><b>&nbsp;&nbsp;&nbsp;Configuración</b>: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + @ConfiguracionMontaje +
'<br/><br/><span style="color: #1F497D;"><b>Situación del bloqueo</b></span>: &nbsp;&nbsp;' + @SituacionBloqueoGrua +
'<br/><br/>Por favor, considere si hay que mantenerlo o quitarlo.
<br/><br/>Saludos,'

EXEC msdb.dbo.sp_send_dbmail	@profile_name = 'AlertasAplicacion_Prototipos',
								@importance = 'High',
								@recipients = @Email,
								@subject = @asunto,
								@body_format = 'HTML',
								@body = @msg
Set @i = @i -1
END
GO
/****** Object:  Table [dbo].[tSeguridad]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tSeguridad](
	[IdPrototipo] [int] NOT NULL,
	[IdCheckList] [int] NOT NULL,
	[IdUsuario] [varchar](50) NOT NULL,
	[SeAcepta] [bit] NULL,
	[item1] [bit] NULL,
	[item2] [bit] NULL,
	[item3] [bit] NULL,
	[item4] [bit] NULL,
	[item5] [bit] NULL,
	[item6] [bit] NULL,
	[item7] [bit] NULL,
	[item8] [bit] NULL,
	[item9] [bit] NULL,
	[item10] [bit] NULL,
	[item11] [bit] NULL,
	[item12] [bit] NULL,
	[item13] [bit] NULL,
	[item14] [bit] NULL,
	[item15] [bit] NULL,
	[item16] [bit] NULL,
	[prohib1] [bit] NULL,
	[prohib2] [bit] NULL,
	[prohib3] [bit] NULL,
 CONSTRAINT [PK_tSeguridad] PRIMARY KEY CLUSTERED 
(
	[IdCheckList] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tPrototipoArchivo]    Script Date: 06/08/2017 10:25:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tPrototipoArchivo](
	[IdPrototipo] [int] NOT NULL,
	[IdArch] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Tipo] [varchar](256) NULL,
	[Lenght] [money] NULL,
	[Contenido] [varbinary](max) NULL,
	[FechaAdd] [datetime] NULL,
	[UsuarioAdd] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_tPrototipoArchivo] PRIMARY KEY CLUSTERED 
(
	[IdPrototipo] ASC,
	[IdArch] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_tPrueba_FechaRegistroPrueba]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tPrueba] ADD  CONSTRAINT [DF_tPrueba_FechaRegistroPrueba]  DEFAULT (getdate()) FOR [FechaRegistroPrueba]
GO
/****** Object:  Default [DF_tPrueba_Deficiencia]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tPrueba] ADD  CONSTRAINT [DF_tPrueba_Deficiencia]  DEFAULT ((0)) FOR [Deficiencia]
GO
/****** Object:  Default [DF_tFase_EsInicial]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFase] ADD  CONSTRAINT [DF_tFase_EsInicial]  DEFAULT ((1)) FOR [EsInicial]
GO
/****** Object:  Default [DF_tFase_Deficiencia]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFase] ADD  CONSTRAINT [DF_tFase_Deficiencia]  DEFAULT ((0)) FOR [Deficiencia]
GO
/****** Object:  Default [DF_tDeficiencia_Fecha]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia] ADD  CONSTRAINT [DF_tDeficiencia_Fecha]  DEFAULT (getdate()) FOR [Fecha]
GO
/****** Object:  Default [DF_tDeficiencia_Bloqueo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia] ADD  CONSTRAINT [DF_tDeficiencia_Bloqueo]  DEFAULT ((0)) FOR [Bloqueo]
GO
/****** Object:  Default [DF_tDeficiencia_FechaDesbloqueo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia] ADD  CONSTRAINT [DF_tDeficiencia_FechaDesbloqueo]  DEFAULT (getdate()) FOR [FechaDesbloqueo]
GO
/****** Object:  Default [DF_tDeficienciaPrueba_Fecha]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba] ADD  CONSTRAINT [DF_tDeficienciaPrueba_Fecha]  DEFAULT (getdate()) FOR [Fecha]
GO
/****** Object:  Default [DF_tDeficienciaPrueba_Bloqueo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba] ADD  CONSTRAINT [DF_tDeficienciaPrueba_Bloqueo]  DEFAULT ((0)) FOR [Bloqueo]
GO
/****** Object:  Default [DF_tDeficienciaPrueba_FechaDesbloqueo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba] ADD  CONSTRAINT [DF_tDeficienciaPrueba_FechaDesbloqueo]  DEFAULT (getdate()) FOR [FechaDesbloqueo]
GO
/****** Object:  Default [DF_tPrototipo_FechaCreacion]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipo] ADD  CONSTRAINT [DF_tPrototipo_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
/****** Object:  Default [DF_tPrototipo_Estado]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipo] ADD  CONSTRAINT [DF_tPrototipo_Estado]  DEFAULT ((1)) FOR [Estado]
GO
/****** Object:  ForeignKey [FK_tPruebaArchivo_tPrueba]    Script Date: 06/08/2017 10:25:36 ******/
ALTER TABLE [dbo].[tPruebaArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tPruebaArchivo_tPrueba] FOREIGN KEY([IdPrueba])
REFERENCES [dbo].[tPrueba] ([IdPrueba])
GO
ALTER TABLE [dbo].[tPruebaArchivo] CHECK CONSTRAINT [FK_tPruebaArchivo_tPrueba]
GO
/****** Object:  ForeignKey [FK_tFaseArchivo_tFase]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tFaseArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tFaseArchivo_tFase] FOREIGN KEY([IdFase])
REFERENCES [dbo].[tFase] ([IdFase])
GO
ALTER TABLE [dbo].[tFaseArchivo] CHECK CONSTRAINT [FK_tFaseArchivo_tFase]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPruArchivo_tDeficienciaPrueba]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tDeficienciaPruArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPruArchivo_tDeficienciaPrueba] FOREIGN KEY([IdDeficiencia])
REFERENCES [dbo].[tDeficienciaPrueba] ([IdDeficiencia])
GO
ALTER TABLE [dbo].[tDeficienciaPruArchivo] CHECK CONSTRAINT [FK_tDeficienciaPruArchivo_tDeficienciaPrueba]
GO
/****** Object:  ForeignKey [FK_tDeficienciaArchivo_tDeficiencia]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tDeficienciaArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaArchivo_tDeficiencia] FOREIGN KEY([IdDeficiencia])
REFERENCES [dbo].[tDeficiencia] ([IdDeficiencia])
GO
ALTER TABLE [dbo].[tDeficienciaArchivo] CHECK CONSTRAINT [FK_tDeficienciaArchivo_tDeficiencia]
GO
/****** Object:  ForeignKey [FK_tTratamientoPieza_tFase]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tTratamientoPieza]  WITH CHECK ADD  CONSTRAINT [FK_tTratamientoPieza_tFase] FOREIGN KEY([IdFase])
REFERENCES [dbo].[tFase] ([IdFase])
GO
ALTER TABLE [dbo].[tTratamientoPieza] CHECK CONSTRAINT [FK_tTratamientoPieza_tFase]
GO
/****** Object:  ForeignKey [FK_tPrueba_tPrototipo]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tPrueba_tPrototipo] FOREIGN KEY([IdPrototipo])
REFERENCES [dbo].[tPrototipo] ([IdPrototipo])
GO
ALTER TABLE [dbo].[tPrueba] CHECK CONSTRAINT [FK_tPrueba_tPrototipo]
GO
/****** Object:  ForeignKey [FK_tPrueba_tUsuarios]    Script Date: 06/08/2017 10:25:37 ******/
ALTER TABLE [dbo].[tPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tPrueba_tUsuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tPrueba] CHECK CONSTRAINT [FK_tPrueba_tUsuarios]
GO
/****** Object:  ForeignKey [FK_tFase_tUsuario]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFase]  WITH CHECK ADD  CONSTRAINT [FK_tFase_tUsuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tFase] CHECK CONSTRAINT [FK_tFase_tUsuario]
GO
/****** Object:  ForeignKey [FK_tFase_tUsuario1]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFase]  WITH CHECK ADD  CONSTRAINT [FK_tFase_tUsuario1] FOREIGN KEY([IdUsuarioDesmontaje])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tFase] CHECK CONSTRAINT [FK_tFase_tUsuario1]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tFase]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tFase] FOREIGN KEY([IdFase])
REFERENCES [dbo].[tFase] ([IdFase])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tFase]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tPrototipo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tPrototipo] FOREIGN KEY([IdPrototipo])
REFERENCES [dbo].[tPrototipo] ([IdPrototipo])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tPrototipo]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tUsuario]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tUsuario] FOREIGN KEY([IdUsuarioCreador])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tUsuario]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tUsuario1]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tUsuario1] FOREIGN KEY([IdUsuarioDesbloqueo])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tUsuario1]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tUsuario2]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tUsuario2] FOREIGN KEY([IdUsuarioResolucion])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tUsuario2]
GO
/****** Object:  ForeignKey [FK_tDeficiencia_tUsuarios]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficiencia]  WITH CHECK ADD  CONSTRAINT [FK_tDeficiencia_tUsuarios] FOREIGN KEY([IdUsuarioBloqueo])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficiencia] CHECK CONSTRAINT [FK_tDeficiencia_tUsuarios]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tPrototipo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tPrototipo] FOREIGN KEY([IdPrototipo])
REFERENCES [dbo].[tPrototipo] ([IdPrototipo])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tPrototipo]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tPrueba]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tPrueba] FOREIGN KEY([IdPrueba])
REFERENCES [dbo].[tPrueba] ([IdPrueba])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tPrueba]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tUsuario]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tUsuario] FOREIGN KEY([IdUsuarioBloqueo])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tUsuario]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tUsuario1]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tUsuario1] FOREIGN KEY([IdUsuarioDesbloqueo])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tUsuario1]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tUsuario2]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tUsuario2] FOREIGN KEY([IdUsuarioResolucion])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tUsuario2]
GO
/****** Object:  ForeignKey [FK_tDeficienciaPrueba_tUsuario3]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tDeficienciaPrueba]  WITH CHECK ADD  CONSTRAINT [FK_tDeficienciaPrueba_tUsuario3] FOREIGN KEY([IdUsuarioCreador])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tDeficienciaPrueba] CHECK CONSTRAINT [FK_tDeficienciaPrueba_tUsuario3]
GO
/****** Object:  ForeignKey [FK_tPruebaAfecta_tDeficienciaPrueba]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPruebaAfecta]  WITH CHECK ADD  CONSTRAINT [FK_tPruebaAfecta_tDeficienciaPrueba] FOREIGN KEY([IdDeficienciaPru])
REFERENCES [dbo].[tDeficienciaPrueba] ([IdDeficiencia])
GO
ALTER TABLE [dbo].[tPruebaAfecta] CHECK CONSTRAINT [FK_tPruebaAfecta_tDeficienciaPrueba]
GO
/****** Object:  ForeignKey [FK_tPruebaAfecta_tSistema]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPruebaAfecta]  WITH CHECK ADD  CONSTRAINT [FK_tPruebaAfecta_tSistema] FOREIGN KEY([IdSistema])
REFERENCES [dbo].[tSistema] ([IdSistema])
GO
ALTER TABLE [dbo].[tPruebaAfecta] CHECK CONSTRAINT [FK_tPruebaAfecta_tSistema]
GO
/****** Object:  ForeignKey [FK_tFaseAfecta_tDeficiencia]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFaseAfecta]  WITH CHECK ADD  CONSTRAINT [FK_tFaseAfecta_tDeficiencia] FOREIGN KEY([IdDeficienciaFas])
REFERENCES [dbo].[tDeficiencia] ([IdDeficiencia])
GO
ALTER TABLE [dbo].[tFaseAfecta] CHECK CONSTRAINT [FK_tFaseAfecta_tDeficiencia]
GO
/****** Object:  ForeignKey [FK_tFaseAfecta_tSistema]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tFaseAfecta]  WITH CHECK ADD  CONSTRAINT [FK_tFaseAfecta_tSistema] FOREIGN KEY([IdSistema])
REFERENCES [dbo].[tSistema] ([IdSistema])
GO
ALTER TABLE [dbo].[tFaseAfecta] CHECK CONSTRAINT [FK_tFaseAfecta_tSistema]
GO
/****** Object:  ForeignKey [FK_tPrototipo_tUsuario]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipo]  WITH CHECK ADD  CONSTRAINT [FK_tPrototipo_tUsuario] FOREIGN KEY([IdUsuarioCreador])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tPrototipo] CHECK CONSTRAINT [FK_tPrototipo_tUsuario]
GO
/****** Object:  ForeignKey [FK_tPrototipo_tUsuario1]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipo]  WITH CHECK ADD  CONSTRAINT [FK_tPrototipo_tUsuario1] FOREIGN KEY([IdUsuarioCierre])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tPrototipo] CHECK CONSTRAINT [FK_tPrototipo_tUsuario1]
GO
/****** Object:  ForeignKey [FK_tPrototipo_tUsuarios]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipo]  WITH CHECK ADD  CONSTRAINT [FK_tPrototipo_tUsuarios] FOREIGN KEY([Responsable])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tPrototipo] CHECK CONSTRAINT [FK_tPrototipo_tUsuarios]
GO
/****** Object:  ForeignKey [FK_tSeguridad_tPrototipo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tSeguridad]  WITH CHECK ADD  CONSTRAINT [FK_tSeguridad_tPrototipo] FOREIGN KEY([IdPrototipo])
REFERENCES [dbo].[tPrototipo] ([IdPrototipo])
GO
ALTER TABLE [dbo].[tSeguridad] CHECK CONSTRAINT [FK_tSeguridad_tPrototipo]
GO
/****** Object:  ForeignKey [FK_tSeguridad_tUsuario]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tSeguridad]  WITH CHECK ADD  CONSTRAINT [FK_tSeguridad_tUsuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[tUsuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[tSeguridad] CHECK CONSTRAINT [FK_tSeguridad_tUsuario]
GO
/****** Object:  ForeignKey [FK_tPrototipoArchivo_tPrototipo]    Script Date: 06/08/2017 10:25:38 ******/
ALTER TABLE [dbo].[tPrototipoArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tPrototipoArchivo_tPrototipo] FOREIGN KEY([IdPrototipo])
REFERENCES [dbo].[tPrototipo] ([IdPrototipo])
GO
ALTER TABLE [dbo].[tPrototipoArchivo] CHECK CONSTRAINT [FK_tPrototipoArchivo_tPrototipo]
GO
