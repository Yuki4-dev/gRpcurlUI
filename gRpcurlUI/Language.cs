using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRpcurlUI
{
    public class Language
    {
        public static readonly Language Default = new ();

        public string Error => "エラー";

        public SettingPageLanguage SettingPage { get; } = new SettingPageLanguage();

        public ProjectTabPageLanguage ProjectTabPage { get; } = new ProjectTabPageLanguage();

        public ProtoImportPageLanguage ProtoImportPage { get; } = new ProtoImportPageLanguage();

        public GrpcProjectLanguage GrpcProject { get; } = new GrpcProjectLanguage();

        public CurlProjectLanguage CurlProject { get; } = new CurlProjectLanguage();

        public class SettingPageLanguage
        {
            public string SettingTitle => "設定";

            public string About => "About...";

            public string OpenSource => "ソースコード";

            public string Version => "gRpcurlUI Ver 1.0.0 (Preview)";

            public string GrpcurlSettingTitle => "grpcurl設定";

            public string FontSettingTitle => "フォント設定";

            public string BrushSettingTitle => "カラー設定";

            public string FontSize => "フォントサイズ";

            public string GrpcExePath => "grpcurl.exe パス";

            public string WindowBackground => "ウィンドウ背景色";

            public string PageBackground => "ページ背景色";

            public string PageForeground => "ページ色";

            public string BorderBrush => "線色";

            public string IconBrush => "アイコン色";

            public string EditAreaTextBoxBrush => "編集用テキストボックス色";

            public string TextBoxSelectBrush => "テキストボックス選択色";

            public string MouseOverBackground => "マウスオーバー背景色";

            public string SelectedBackground => "選択時色";

            public string ScrollBarTabBrush => "スクロールバータブ色";

            public string SettingExpanderAreaBackGround => "設定項目背景色";

            public string CationColor => "注意文字色";
        }

        public class ProjectTabPageLanguage
        {
            public string SaveProject => "プロジェクト保存";

            public string OpenProject => "プロジェクト開く";

            public string Remove => "削除";

            public string RemoveCount => "{0}件のプロジェクトを削除します。";

            public string ProjectNothing => "プロジェクトが選択されていません。";

            public string Send => "送信";

            public string ContinueQ => "実行しますか？";

            public string CancelSendingQ => "送信をキャンセルしますか？";
        }

        public class ProtoImportPageLanguage
        {

        }

        public class GrpcProjectLanguage
        {
            public string EndPoint => "エンドポイント";

            public string Option => "オプション";

            public string Service => "サービス";

            public string NotExist => "{0}は存在しません。";

            public string EndPointBlank => "エンドポイントが空白です。";

            public string NonJsonType => "JsonがGrpcProjectではありません。";

            public string ReadProto => "プロト読み込み（ベータ）";

            public string NewProject => "新規プロジェクト";

            public string VersionError => "読み込みプロジェクトのバージョンが違います。（読み込みプロジェクトのバージョン：{0}　対応のバージョン：{1}）";

            public string ProjectTypeError => "プロジェットのタイプが違います。（読み込みプロジェクトのタイプ：{0}　対応のタイプ：{1}）";

            public string ProjectNothing => "プロジェクトがありません。";
        }

        public class CurlProjectLanguage
        {
            public string EndPoint => "エンドポイント";

            public string Option => "オプション";

            public string NotExist => "{0}は存在しません。";

            public string EndPointBlank => "エンドポイントが空白です。";

            public string NonJsonType => "JsonがCurlProjectではありません。";

            public string NewProject => "新規プロジェクト";

            public string VersionError => "読み込みプロジェクトのバージョンが違います。（読み込みプロジェクトのバージョン：{0}　対応のバージョン：{1}）";

            public string ProjectTypeError => "プロジェットのタイプが違います。（読み込みプロジェクトのタイプ：{0}　対応のタイプ：{1}）";

            public string ProjectNothing => "プロジェクトがありません。";
        }
    }
}
