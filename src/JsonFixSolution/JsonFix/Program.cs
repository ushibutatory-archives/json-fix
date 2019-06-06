using System;
using System.Text;
using System.Windows.Forms;

namespace JsonFix
{
	/// <summary>
	/// JSON整形ツール
	/// </summary>
	public class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		[STAThread]
		public static void Main()
		{
			try
			{
				var data = Clipboard.GetDataObject();
				if (data.GetDataPresent(DataFormats.Text))
				{
					// クリップボードのテキストを取得
					var source = Clipboard.GetText();

					var result = new StringBuilder();

					// インデントレベル
					var indent = 0;

					// インデント追加
					var addIndent = new Func<String>(() => { return new String('\t', indent); });

					// テキスト中かどうか
					var inText_single = false;
					var inText_double = false;

					foreach (var character in source)
					{
						if (inText_double)
						{
							// ダブルクォーテーション中の場合
							switch (character)
							{
								case '"':
									inText_double = !inText_double;
									result.Append(character.ToString());
									break;

								default:
									// テキスト中の文字は編集しない
									result.Append(character.ToString());
									break;
							}
						}
						else if (inText_single)
						{
							// シングルクォーテーション中の場合
							switch (character)
							{
								case '\'':
									inText_single = !inText_single;
									result.Append(character.ToString());
									break;

								default:
									// テキスト中の文字は編集しない
									result.Append(character.ToString());
									break;
							}
						}
						else
						{
							// テキスト外の場合
							switch (character)
							{
								case '{':
									indent++;
									result.Append(character.ToString());
									result.AppendLine();
									result.Append(addIndent());
									break;

								case '[':
									indent++;
									result.Append(character.ToString());
									result.AppendLine();
									result.Append(addIndent());
									break;

								case '}':
									indent--;
									result.AppendLine();
									result.Append(addIndent());
									result.Append(character.ToString());
									break;

								case ']':
									indent--;
									result.AppendLine();
									result.Append(addIndent());
									result.Append(character.ToString());
									break;

								case '"':
									inText_double = !inText_double;
									result.Append(character.ToString());
									break;

								case '\'':
									inText_single = !inText_single;
									result.Append(character.ToString());
									break;

								case ':':
									result.Append(" : ");
									break;

								case ' ':
									// テキスト外のスペースは除去（Appendしない）
									break;

								case ',':
									result.AppendLine();
									result.Append(addIndent());
									result.Append(", ");
									break;

								default:
									result.Append(character.ToString());
									break;
							}
						}
					}

					// クリップボードに貼り付け
					Clipboard.SetText(result.ToString());
				}
			}
			catch (Exception ex)
			{
				Clipboard.SetText(ex.Message);
			}
		}
	}
}
