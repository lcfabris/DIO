using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace DIO.Series
{
    class Program
    {
        static SerieRepositorio repositorio = new SerieRepositorio();
        static void Main(string[] args)
        {
        	Console.Clear();			
		    string opcaoUsuario = ObterOpcaoUsuario();

			while (opcaoUsuario.ToUpper() != "X")
			{
				switch (opcaoUsuario)
				{
					case "1":
						ListarSeries();
						break;
					case "2":
						InserirSerie();
						break;
					case "3":
						AtualizarSerie();
						break;
					case "4":
						ExcluirSerie();
						break;
					case "5":
						VisualizarSerie();
						break;
					case "C":
						Console.Clear();
						break;
					default:
						//throw new ArgumentOutOfRangeException();
						Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Opção inválida! Informe de 1 a 5, C para limpar o console ou X para sair.");
                        Console.WriteLine();
                        break;
				}
				opcaoUsuario = ObterOpcaoUsuario();
			}
			Console.WriteLine("Obrigado por utilizar nossos serviços.");
			Console.WriteLine(" Obrigado por utilizar nossos serviços.");
			Console.WriteLine("  Obrigado por utilizar nossos serviços.");
        }

        private static void ListarSeries()
		{
			Console.WriteLine("Listar séries");
			Console.WriteLine("=============");
			Console.WriteLine();

			var lista = repositorio.Lista();

			if (lista.Count == 0)
			{
				Console.WriteLine("Nenhuma série cadastrada.");
				Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>  Digite algo para continuar");
				Console.ReadLine();
				Console.Clear();				
				return;
			}
			foreach (var serie in lista)
			{
                var excluido = serie.retornaExcluido();                
				Console.WriteLine("#ID {0}: - {1} {2}", serie.retornaId(), serie.retornaTitulo(), (excluido ? "*Excluído*" : ""));
			}
			Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>  Digite algo para continuar");
			Console.ReadLine();
			Console.Clear();	
		}

        private static void InserirSerie()
		{
			Console.WriteLine("Inserir nova série");

			// https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
			// https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
			foreach (int i in Enum.GetValues(typeof(Genero)))
			{
				Console.WriteLine("{0}-{1}", i, Enum.GetName(typeof(Genero), i));
			}
			Console.Write("Digite o gênero entre as opções acima: ");
			int entradaGenero = int.Parse(Console.ReadLine());

			Console.Write("Digite o Título da Série: ");
			string entradaTitulo = Console.ReadLine();

			Console.Write("Digite o Ano de Início da Série: ");
			int entradaAno = int.Parse(Console.ReadLine());

			Console.Write("Digite a Descrição da Série: ");
			string entradaDescricao = Console.ReadLine();

			Serie novaSerie = new Serie(id: repositorio.ProximoId(),
										genero: (Genero)entradaGenero,
										titulo: entradaTitulo,
										ano: entradaAno,
										descricao: entradaDescricao);

			repositorio.Insere(novaSerie);
			Console.WriteLine();
			Console.WriteLine("Série inserida. Tecle algo para continuar!");
			Console.ReadLine();
			Console.Clear();
		}

        private static void AtualizarSerie()
		{
			Console.Write("Digite o id da série: ");
			
			int indiceSerie = int.Parse(Console.ReadLine());
			if (checaId(indiceSerie) == false)	
			{
				return;
			}

			// https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
			// https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
			foreach (int i in Enum.GetValues(typeof(Genero)))
			{
				Console.WriteLine("{0}-{1}", i, Enum.GetName(typeof(Genero), i));
			}
			Console.Write("Digite o gênero entre as opções acima: ");
			string gen =  Console.ReadLine();
			int entradaGenero;
			if (gen != "")
			{
				entradaGenero = int.Parse(gen);
			} else {
				Console.WriteLine("Opção inválida. Tecle algo para retornar");
				Console.ReadLine();
				return;
			}

			//int entradaGenero = int.Parse(Console.ReadLine());

			Console.Write("Digite o Título da Série: ");
			string entradaTitulo = Console.ReadLine();

			Console.Write("Digite o Ano de Início da Série: ");
			int entradaAno = int.Parse(Console.ReadLine());

			Console.Write("Digite a Descrição da Série: ");
			string entradaDescricao = Console.ReadLine();

			Serie atualizaSerie = new Serie(id: indiceSerie,
										genero: (Genero)entradaGenero,
										titulo: entradaTitulo,
										ano: entradaAno,
										descricao: entradaDescricao);

			repositorio.Atualiza(indiceSerie, atualizaSerie);
		}

		private static void ExcluirSerie()
		{
			Console.Write("Digite o id da série: ");
			int indiceSerie;
			bool resultado = int.TryParse(Console.ReadLine(), out indiceSerie);
			if (resultado)
			{
				if (checaId(indiceSerie) == false)	
				{
					return;
				}
				repositorio.Exclui(indiceSerie);				
			} else {
				Console.Write("Opção invalida. Tecle algo para retornar!");
				Console.ReadLine();
				return;
			}
		}

        private static void VisualizarSerie()
		{
			Console.Write("Digite o id da série: ");
			int indiceSerie;
			bool resultado = int.TryParse(Console.ReadLine(), out indiceSerie);
			if (resultado)
			{
				if (checaId(indiceSerie) == false)	
				{
					return;
				}
				var serie = repositorio.RetornaPorId(indiceSerie);
				Console.WriteLine(serie);
				Console.WriteLine();
				Console.WriteLine("Tecle algo para continuar.");
				Console.ReadLine();
				Console.Clear();
			} else {
				Console.Write("Opção invalida. Tecle algo para retornar!");
				Console.ReadLine();
				return;
			}
		}

        private static string ObterOpcaoUsuario()
		{
			Console.WriteLine();
			Console.WriteLine("DIO Séries a seu dispor!!!");
			Console.WriteLine("Informe a opção desejada:");
			Console.WriteLine();

			Console.WriteLine("1- Listar séries");
			Console.WriteLine("2- Inserir nova série");
			Console.WriteLine("3- Atualizar série");
			Console.WriteLine("4- Excluir série");
			Console.WriteLine("5- Visualizar série");
			Console.WriteLine("C- Limpar Tela");
			Console.WriteLine("X- Sair");
			Console.WriteLine();

			string opcaoUsuario = Console.ReadLine().ToUpper();
			Console.WriteLine();
			return opcaoUsuario;
		}

		private static bool checaId(int id)
		{
			if (id > repositorio.ProximoId())
			{
				Console.Write("Não existe série com o ID informado! Tecle algo para continuar.");
				Console.ReadLine();
				Console.Clear();
				return false;
			} else {
				return true;
			}
		}

    }
}
