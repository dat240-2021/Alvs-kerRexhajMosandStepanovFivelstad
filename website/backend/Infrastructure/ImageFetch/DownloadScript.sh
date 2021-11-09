#!/bin/bash
ROOTDIR=$1
cd $ROOTDIR

rm -rf "DownloadedFiles"
rm -rf "temp"

mkdir "temp"

cd "temp"
wget -O images.zip "https://www.dropbox.com/sh/j80xn9prbw086wc/AAA3hCVfPAsHxNq1Ie0okL-Ta?dl=0"


unzip "images"
unzip "scattered images.zip"

cd ..
mkdir -p "DownloadedFiles"
mv "$ROOTDIR/temp/task 2 scattered images" "$ROOTDIR/temp/Images"
mv "$ROOTDIR/temp/Images" "$ROOTDIR/DownloadedFiles/"
mv "$ROOTDIR/temp/image_mapping.csv" "$ROOTDIR/DownloadedFiles/"
mv "$ROOTDIR/temp/label_mapping.csv" "$ROOTDIR/DownloadedFiles/"
rm -rf "temp"

