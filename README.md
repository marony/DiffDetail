# DiffDetail

　ちょっとC#のソースファイルの行数を数える必要があったので作りました。
　コメントや空行は読み飛ばしますが、比較やその辺のロジックは簡単に追加できるようになっています。

## 使い方

    DiffDetail 比較元ディレクトリ 比較先ディレクトリ [比較するファイル名のパターン]

## 出力形式

コンソールにCSVで下記のように出力されますので適当にリダイレクトしてください。

    比較元ディレクトリ,比較先ディレクトリ
    ファイル名,追加行数,削除行数,前内容,後内容
    …
    …
    …