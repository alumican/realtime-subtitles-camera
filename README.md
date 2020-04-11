# Realtime Subtitles Camera

> オンラインだからできること何かないかなと思って、英語を日本語に、日本語を英語にリアルタイム翻訳して字幕をつけてZoomとかに参加できるシステムを作ってみた。

動画あり

https://twitter.com/alumican_net/status/1248871338745319425

![screenshot](https://user-images.githubusercontent.com/172811/79047898-73326100-7c54-11ea-8f61-84c492d4d30e.png)

以下、必要最低限の説明

# 動作環境

- Windows 10
- Unity 2019.3.8f1
- ウェブカメラ

# 必要なUnity Asset

## TextMesh Pro

リッチな字幕表示用に利用していますが、UnityのデフォルトのTextでも可能です。

## Klak Spout

Unityのカメラを、Spout形式で出力するために必要です。

[こちら](https://github.com/keijiro/KlakSpout)からダウンロードします。

## Speech SDK

Microsoft Cognitive ServicesのUnity用SDKです。Mac非対応。

[こちら](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/from-microphone)の中段の以下のリンクからダウンロード可能です。

> The Speech SDK for Unity is packaged as a Unity asset package (.unitypackage). Download it from here.

## フォント「しねきゃぷしょん」
フォントは何でもよいのですが、デモで使用している再配布可能なフォント「しねきゃぷしょん」をTextMesh Proで利用可能なアトラス化したものを用意したので、それを利用する場合は以下のファイルをダウンロードして`Assets/Fonts`に展開します。

https://www.dropbox.com/s/m1iic14a1j8h2vy/Fonts.zip?dl=0

# 必要な外部アプリケーション

## Spout

UnityからSpout映像をZoomなどのウェブカメラ入力として繋ぐために必要です。

[こちら](https://spout.zeal.co/download-software/)からダウンロードして、インストーラー内のSpoutCamにチェックを入れてインストールします。

上記サイトは重たいので、[こちら](https://github.com/leadedge/Spout2/tree/master/INSTALLATIONS/SPOUT%202)の`SpoutSetup_V2.005.zip`でも大丈夫です。

# Speech APIキーの取得

[Azureのポータル](https://portal.azure.com/)でSpeech APIをサブスクリプションし（無料枠のあるF0で契約したほうがいいです）、APIキーを発行する。リージョンは東日本でいいと思います。

管理画面でのAPIキー（Key1）と、エンドポイントのサブドメイン部分のリージョン部分（japaneastなど）をコピーしておきます。

# 操作方法

1. UnityのMainシーンをプレビューする
2. スペースキーを押して設定画面を表示する
3. APIキー入力、リージョン入力、デバイス選択、モード選択（日→英 or 英→日）をおこない、Startボタンを押す
4. スペースキーを押して設定画面を閉じる
5. Zoomなどのカメラで`Spout Cam`を選択するとUnityのカメラ映像がストリーミングされる
6. 終了時は設定画面を開いてStopボタンを押すか、プレビューを終了する

StartからStopまでの時間でAPIを消化するので、Stopのし忘れにご注意ください。

SnapCameraと組み合わせる場合は、Unityの設定画面のデバイス選択で`Snap Camera`を選択してください。

# 参考になりそうなリンク

[Microsoft Azure の無料アカウントを作ってみた
](https://qiita.com/shinyay/items/a6106936b4a640ab0dc4)

[Speech Service を無料で試す](https://docs.microsoft.com/ja-jp/azure/cognitive-services/speech-service/get-started)

[TranslationRecognizer Class](https://docs.microsoft.com/en-us/dotnet/api/microsoft.cognitiveservices.speech.translation.translationrecognizer?view=azure-dotnet)

[リージョンとエンドポイント](https://docs.microsoft.com/ja-jp/azure/cognitive-services/speech-service/rest-speech-to-text#regions-and-endpoints)

[Sample Repository for the Microsoft Cognitive Services Speech SDK](https://github.com/Azure-Samples/cognitive-services-speech-sdk)

[Unity内のカメラをZoomのカメラとして使う方法](https://qiita.com/yukihiko_a/items/81883d2a01af6e4637e5)
