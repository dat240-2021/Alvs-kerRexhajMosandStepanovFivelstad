from csv import reader
import sqlite3
import os

conn = sqlite3.connect('..\\backend\\Infrastructure\\Data\game.db')
curr = conn.cursor()

BASEFOLDER = '.\project_images\images\scattered images\\task 2 scattered images'

def read_csv_image_mapping(file):
    csv_list = []
    with open(file, 'r') as read_obj:
        csv_reader = reader(read_obj)
        for row in csv_reader:
            temp = row[0].split()
            temp_tuple = (temp[0], int(temp[1]))
            csv_list.append(temp_tuple)
    return csv_list

def read_csv_label_mapping(file):
    csv_map = {}
    label_map = {}
    with open(file, 'r') as read_obj:
        csv_reader = reader(read_obj)
        for row in csv_reader:
            temp = row[0].split(";")
            label_map[temp[1]] = 1
            temp2 = temp[0].split()
            if temp2[0] == 'ï»¿0':
                temp2[0] = '0'
            word = ""
            for i in range(len(temp2)-1):
                word += temp2[1+i] + " "
            csv_map[int(temp2[0])] =[word, temp[1]]
    return csv_map, label_map

def find_image_folder_names():
    folder_list = [f.path for f in os.scandir(BASEFOLDER) if f.is_dir()]
    for i in range(len(folder_list)):
        folder_name = folder_list[i][-33:]
        folder_list[i] = folder_name
    return  folder_list


def insert_image_files(last_id, folder_name):
   folder = BASEFOLDER + "\\" + folder_name
   file_list = [ f for f in os.listdir(folder) if os.path.isfile(os.path.join(folder,f)) ]
   for i in range(len(file_list)):
       if file_list[i].endswith('.png'):
           sequenceNumber = int(file_list[i].strip('.png'))
           with open(BASEFOLDER + "\\" + folder_name + "\\" + file_list[i], 'rb') as file:
                binaryData = file.read()
           curr.execute("INSERT INTO IMAGEPIECES (ImageData, ImageId, SequenceNumber) VALUES (?, ?,?)", (binaryData, last_id, sequenceNumber))

def insert_categories(category_map):
    category_db_ids = {}
    for key in category_map:
        curr.execute("INSERT INTO IMAGECATEGORIES (Category) VALUES (?)", (key,))
        category_db_ids[key] = curr.lastrowid
    return category_db_ids

def execute_sql_statements(image_info,category_map):
    category_db_ids = insert_categories(category_map)
    for i in range(len(image_info)):
        image_name = image_info[i][0]
        category_id = category_db_ids[image_info[i][1]]
        curr.execute("INSERT INTO IMAGE (ImageName, CategoriesId) VALUES (?, ?)", (image_name,category_id))
        insert_image_files(curr.lastrowid,image_info[i][2])

    conn.commit()

def link_image_info(image_list,label_map,folder_names):
    image_info = []
    for i in range(len(folder_names)):
        for j in range(len(image_list)):
            if image_list[j][0] == folder_names[i][:-10]:
                category = label_map[image_list[j][1]][1]
                image_name = label_map[image_list[j][1]][0]
                image_info.append((image_name,category,folder_names[i]))
    return image_info

def find_if_db_tables_exists():
    pass




if __name__ == '__main__':
    file1 = '.\project_images\images\image_mapping.csv'
    image_list = read_csv_image_mapping(file1)
    file2 = '.\project_images\images\label_mapping_with_categories.csv'
    label_map, category_map = read_csv_label_mapping(file2)
    image_folders = find_image_folder_names()
    image_info = link_image_info(image_list,label_map,image_folders)
    execute_sql_statements(image_info, category_map)


