#!/bin/bash
if [[ -d "./project_images" ]]
then
	while true; do
		read -p "This folder already exists, download again? " yn
		case $yn in
			[Yy]* ) rm -rf "project_images"; break;;
			[Nn]* ) exit;;
			* ) echo "Please answer yes or no.";;
   		esac
	done
fi

mkdir "project_images"

cd "project_images"
wget -O images.zip "https://www.dropbox.com/sh/j80xn9prbw086wc/AAA3hCVfPAsHxNq1Ie0okL-Ta?dl=0"


unzip "images"
unzip "scattered images.zip"
