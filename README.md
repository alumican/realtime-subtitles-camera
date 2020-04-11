# Realtime Subtitles Camera

> オンラインだからできること何かないかなと思って、英語を日本語に、日本語を英語にリアルタイム翻訳して字幕をつけてZoomとかに参加できるシステムを作ってみた。

https://twitter.com/alumican_net/status/1248871338745319425

以下、必要最低限の説明

# 動作環境

- Windows 10
- Unity 2019.3.8f1
- ウェブカメラ

# 必要なUnity Asset

## TextMesh Pro

字幕表示用

## Klak Spout

Unityのカメラを、Spoutの出力として設定する

[こちら](https://github.com/keijiro/KlakSpout)からダウンロードする

## Speech SDK

Microsoft Cognitive ServicesのUnity用SDK。Mac非対応。

[こちら](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/from-microphone)の中段の以下のリンクからダウンロード可能

> The Speech SDK for Unity is packaged as a Unity asset package (.unitypackage). Download it from here.

## フォント「しねきゃぷしょん」
フォントはなんでもよいのですが、デモで使用している再配布可能なフォント「しねきゃぷしょん」をTextMesh Proで利用可能なアトラス化したものを用意したので、それを利用する場合は以下のファイルをダウンロードして Assets/Fonts に展開する

https://www.dropbox.com/s/m1iic14a1j8h2vy/Fonts.zip?dl=0

# 必要な外部アプリケーション

## Spout

UnityからSpout映像をZoomなどのウェブカメラ入力として繋ぐために必要

[こちら](https://spout.zeal.co/download-software/)からダウンロードして、インストーラー内のSpoutCamにチェックを入れてインストール。
Microsoft Cognitive ServicesのUnity用SDK。Mac非対応

# Speech APIキーの取得

[Azureのポータル](https://portal.azure.com/)でSpeech APIをサブスクリプションし（無料枠のあるF0で契約したほうがいいです）、APIキーを発行する。

管理画面でのAPIキー（Key1）と、エンドポイントのサブドメイン部分のリージョン部分（japaneastなど）をコピーしておく。

# 操作方法

1. UnityのMainシーンをプレビューする
2. スペースキーを押して設定画面を表示する
3. APIキー入力、リージョン入力、デバイス選択、モード選択（日→英 or 英→日）をおこない、Startボタンを押す
4. スペースキーを押して設定画面を閉じる
5. 終了時は設定画面を開いてStopボタンを押す

StartからStopまでの時間でAPIを消化するので、Stopのし忘れにご注意ください。
