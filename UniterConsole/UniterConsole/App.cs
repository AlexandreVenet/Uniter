using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UniterConsole
{
	internal class App
	{
		#region Champs

		private string _titreApp;
		private string[] _args;
		private Modele _donnees;
		private StringBuilder _sb;

		#endregion



		#region Constructeurs

		public App()
		{
			ParametrerConsole();
		}

		#endregion



		#region Méthodes

		public void ParametrerConsole()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			_titreApp = typeof(App).Namespace;
			Console.Title = _titreApp;
		}

		public void Demarrer(string[] args)
		{
			_args = args;

			EcrireTitre(_titreApp);

			try
			{
				TesterLongueurArguments();
				TesterPremierArgumentNonNullOuVide();
				ExecuterRoute().Wait();
				TesterDonnees();
				TesterCheminFichierSortie();
				CreerDonneesCSV();
				CreerFichierCSV().Wait();
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine($"Exception [{e.GetType()}] {e.Message}");
				Console.ResetColor();
			}
			finally
			{
				EcrireTitre("Fin du programme");
			}
		}

		private void TesterLongueurArguments()
		{
			Console.WriteLine("Tester le nombre d'arguments.");

			if (_args == null || _args.Length == 0)
			{
				throw new Exception("Renseigner l'URL du fichier de test.");
			}
		}

		private void TesterPremierArgumentNonNullOuVide()
		{
			Console.WriteLine("Tester le premier argument.");

			if (string.IsNullOrEmpty(_args[0]))
			{
				throw new Exception("Renseigner une URL valide.");
			}
		}

		private async Task ExecuterRoute()
		{
			Console.WriteLine("Exécuter la route : " + _args[0]);

			using (HttpClient httpClient = new())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.Timeout = TimeSpan.FromSeconds(10);

				// Pas besoin d'envelopper ce qui suit en try...catch car la méthode appelante le fait déjà.

				HttpResponseMessage reponse = await httpClient.GetAsync(_args[0]);
				if (reponse.IsSuccessStatusCode)
				{
					string contenu = await reponse.Content.ReadAsStringAsync();
					_donnees = JsonSerializer.Deserialize<Modele>(contenu);
				}
				else
				{
					throw new Exception("Erreur de chargement : " + reponse.StatusCode);
				}
			}
		}

		private void TesterDonnees()
		{
			Console.WriteLine("Tester les données.");

			if (_donnees == null)
			{
				throw new Exception("Pas de données.");
			}
		}

		private void TesterCheminFichierSortie()
		{
			Console.WriteLine("Tester le chemin de fichier de sortie.");

			string cheminDossierSortie = Path.GetDirectoryName(_donnees.CheminFichierSortie);

			if (!Directory.Exists(cheminDossierSortie))
			{
				Directory.CreateDirectory(cheminDossierSortie);
				Console.WriteLine("Le dossier de sortie a été créé.");
			}
			else
			{
				Console.WriteLine("Le dossier de sortie existe déjà.");
			}
			Console.WriteLine("Dossier de sortie : " + cheminDossierSortie);
			Console.WriteLine("Fichier de sortie : " + Path.GetFileName(_donnees.CheminFichierSortie));
		}

		private void CreerDonneesCSV()
		{
			Console.WriteLine("Construire les données pour le fichier CSV.");

			_sb = new();

			for (int i = 0; i < _donnees.Resultats.Length; i++)
			{
				string ligne = string.Join(';', _donnees.Resultats[i]);
				_sb.Append(ligne);
				_sb.Append(Environment.NewLine);
			}
		}

		private async Task CreerFichierCSV()
		{
			Console.WriteLine("Générer le fichier CSV.");

			FileStream fichier = File.Create(_donnees.CheminFichierSortie);

			using (StreamWriter writer = new(fichier, encoding: Encoding.UTF8))
			{
				await writer.WriteAsync(_sb.ToString());
			}
		}

		#endregion



		#region Méthodes IHM

		private void EcrireTitre(string texte)
		{
			int nombreCaracteres = texte.Length;
			string souligne = ObtenirSouligne(nombreCaracteres);

			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(souligne);
			Console.WriteLine(texte);
			Console.WriteLine(souligne);
			Console.ResetColor();
		}

		private string ObtenirSouligne(int nombreCaracteres)
		{
			return new string('=', nombreCaracteres);
		}

		#endregion
	}
}
