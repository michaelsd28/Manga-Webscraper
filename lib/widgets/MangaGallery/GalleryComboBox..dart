import 'package:flutter/material.dart';

import '../../model/ChapterList.dart';

class GalleryComboBox extends StatefulWidget {

  final ChapterList chapterList;
  final String currentChapterName;
  final String chapterLink;

  const GalleryComboBox(
      {Key? key, required this.chapterList, required this.currentChapterName, required this.chapterLink})
      : super(key: key);

  @override
  State<GalleryComboBox> createState() => _GalleryComboBoxState();
}

class _GalleryComboBoxState extends State<GalleryComboBox> {
  String dropdownValue = 'Select a chapter';

  String GetMangaType(String mangaName) {

    print ("called GetMangaType");
    if (mangaName.contains("One") || mangaName.contains("one")) {
      return "OnePieceList";

    } else if (mangaName.contains("hero") || mangaName.contains("boku")) {
      return "BokuNoHeroList";
    } else if (mangaName.contains("boruto")|| mangaName.contains("boru")) {
      return "BorutoList";
    }
    return "";
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.transparent,
      child: DropdownButton<String>(
        value: dropdownValue,
        icon: const Icon(
          Icons.arrow_drop_down,
        ),
        elevation: 16,
        dropdownColor: Colors.black,
        style: const TextStyle(color: Colors.blue),
        underline: Container(
          height: 2,
          color: Colors.blue,
        ),
        onChanged: (String? newValue) {

          setState(() {
            dropdownValue = newValue!;

            String mangaType = GetMangaType(widget.chapterLink);
            String LinkTo =   widget.chapterLink;

           /// navigate to next link
          widget.chapterList.MangaNames.asMap().forEach((index, value) {
            if (value == newValue) {
                    LinkTo = widget.chapterList.LinkName[index].replaceAll(" ", "");
            } else {
              return;
            }
          });

            String linkTO= "/$mangaType/$LinkTo";

           Navigator.pushNamed(context, linkTO);


          });


        },
        items: <String>['Select a chapter', ...widget.chapterList.MangaNames]
            .map<DropdownMenuItem<String>>((String value) {
          return DropdownMenuItem<String>(
            value: value,
            child: Padding(padding: EdgeInsets.all(8), child: Text(value, style: TextStyle(fontSize: 16),),),
          );
        }).toList(),
      ),
    );
  }
}
