#!/bin/bash
if [[ -d "./temp" ]]
then
	exit 0
fi

mkdir "temp"

cd "temp"
wget -O images.zip "https://www.dropbox.com/sh/j80xn9prbw086wc/AAA3hCVfPAsHxNq1Ie0okL-Ta?dl=0"


unzip "images"
unzip "scattered images.zip"

cd ..
mkdir "DownloadedFiles"
mkdir "DownloadedFiles/Images"
mv "./temp/task 2 scattered images" "./DownloadedFiles/Images"
mv "./temp/image_mapping.csv" "./DownloadedFiles/"
mv "./temp/label_mapping.csv" "./DownloadedFiles/"
rm -rf "temp"

