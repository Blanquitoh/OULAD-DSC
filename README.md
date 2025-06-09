# 🧠 Caso Práctico 3 – ETL y EDA del dataset OULAD

Maestría en Ciencia de Datos e Inteligencia Artificial (MACDIA)
Materia: Ciencia de Datos I – INF-7303-C1
Profesor: Silverio Del Orbe A.

## 🎯 Objetivo del proyecto
Este repositorio corresponde al tercer caso práctico colaborativo. El objetivo es
exportar el modelo del dataset **OULAD** a **SQL Server**, limpiar la data,
crear las claves primarias y foráneas, generar campos ordinales para variables
categóricas y preparar un `FullDomain` en los niveles detallados de **ASSESS** y
**VLE**. Sobre la base resultante se ejecuta un proceso de **ETL** y un **EDA**
extendido implementado preferiblemente en C#.

## 🧱 Arquitectura de la solución
La solución sigue los principios de *Clean Architecture* y separa la lógica en
capas claras:

- **DataAccess** – DbContext de Entity Framework Core y scripts de migración.
- **Domain** – Entidades con validaciones y reglas de negocio.
- **DataImport** – Lectura de CSV y mapeo de variables categóricas a ordinales.
- **Infrastructure** – Utilidades de logging y carga masiva de datos.
- **Pipeline** – Implementación del proceso ETL orquestado.
- **Tests** – Conjunto de pruebas unitarias para validadores y mapeadores.
- **Eda** – `ExtendedEda` genera matrices de confusión y correlación, además de
  boxplots, histogramas y gráficas de dispersión usando OxyPlot.

## 🧰 Stack Tecnológico

| Capa            | Tecnología                       |
|-----------------|----------------------------------|
| Backend         | .NET 9.0 (consola)               |
| ORM             | Entity Framework Core            |
| Base de Datos   | SQL Server                       |
| EDA             | C# (.NET), ML.NET, OxyPlot       |
| IDE             | Visual Studio 2022 / VS Code     |

## ✅ Criterios de evaluación cubiertos

| Criterio                                                     | Cumplimiento |
|--------------------------------------------------------------|--------------|
| Montar el OULAD en un DBMS                                   | ✅ Script DDL |
| ETL orquestado y utilidades encapsuladas                     | ✅ Implementado |
| EDA extendido (matrices de confusión, correlaciones, etc.)   | ✅ C# |
| Artículo de divulgación con hallazgos                        | ✅ En elaboración |

## 👥 Equipo de trabajo
- Víctor Martín Blanco Núñez
- Anthony Manuel Burgos Reyes

## 📄 Documentación de entrega
El documento en formato `.docx` incluye una presentación del equipo en estilo
APA, un resumen técnico de 250 palabras, capturas y enlaces al repositorio.

## 🚀 Cómo ejecutar el proyecto
1. Clona este repositorio.
2. Ejecuta `dotnet restore` en la raíz para descargar las dependencias.
3. Crea la base de datos destino y ajusta la cadena de conexión en `Program.cs`.
4. Ejecuta `dotnet ef database update` para generar el esquema. Este comando
   también aplica la migración `CreateFullDomainView` que genera la vista
   `FullDomain` consolidando las tablas **ASSESS** y **VLE**.
   Si prefieres ejecutarla manualmente puedes correr:
   ```bash
   sqlcmd -S <server> -d <db> -i sql/create_full_domain.sql
   ```
5. Corre la aplicación en modo ETL:
   ```bash
   dotnet run -- --mode Etl --csv-dir <ruta-a-csv>
   ```
6. Para el análisis exploratorio ejecuta:
   ```bash
   dotnet run -- --mode Eda
   ```
   Las gráficas generadas por `ExtendedEda` se guardarán en la carpeta `plots`.
7. Ejecuta `./test.sh` para construir y correr todas las pruebas.

## 📚 Referencias
- [Entity Framework Core Docs](https://learn.microsoft.com/ef/)
- [SQL Server Documentation](https://learn.microsoft.com/sql/)
