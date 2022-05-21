import 'package:flutter/material.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';


class ChapterButton extends StatelessWidget {
  final String title;
  final String url;
  final String mangaType;
  final List<Color> colors;

  const ChapterButton(
      {Key? key,
        required this.title,
        required this.url,
        required this.mangaType,
        required this.colors})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.all(10),
      child: TextButton(
          onPressed: () {
            //navigate to chapter page

            String newUrl = url.replaceAll(" ", "");

            if (newUrl[0] == " ") {
              newUrl = newUrl.substring(1);
            }

            String linkurl = "/$mangaType/$newUrl";

            if (linkurl[linkurl.length - 1] == '/') {
              linkurl = linkurl.substring(0, linkurl.length - 1);
            }

            print("clicked: $linkurl");

            Navigator.pushNamed(context, linkurl);
          },
          style: ButtonStyle(
            backgroundColor: MaterialStateProperty.all<Color>(
                Colors.black.withOpacity(0.01)),
            foregroundColor: MaterialStateProperty.all<Color>(colors[1]),
            overlayColor:
            MaterialStateProperty.all<Color>(colors[1].withOpacity(0.2)),
          ),
          child: Row(
            children: [
              Padding(
                padding: EdgeInsets.all(10),
                child: Image.asset(
                  'assets/topbar/chapter icon.png',
                  height: 20,
                ),
              ),
              Flexible(

                child: Text(
                  title,
                  overflow: TextOverflow.ellipsis,
                  style: TextStyle(fontSize: 18),
                ),
              ),
              /// delete button
              DeleteButton(),

            ],
          )
      ),
    );
  }
}


class DeleteButton extends StatelessWidget {
  const DeleteButton({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      child:
      IconButton(onPressed: (){
        DatabaseService().refetchImageList("");

      }, icon:
      const Icon(
        Icons.browser_updated,
        color: Colors.white70
      ),
      ),
    );
  }
}
