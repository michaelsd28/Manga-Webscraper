import 'package:flutter/material.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';
import 'package:mymangawebsite/services/GoogleDriveService.dart';
import 'package:mymangawebsite/services/ImageService.dart';
import 'package:mymangawebsite/services/ScrapeService..dart';
import 'package:mymangawebsite/widgets/BodyMangaSite.dart';
import 'package:mymangawebsite/widgets/Footer.dart';
import 'package:mymangawebsite/widgets/MangaGallery/MangaGalleryBody.dart';
import 'package:mymangawebsite/widgets/TopBar.dart';


import 'Routes.dart';

void main() {
  runApp(MangaApp());
}

class MangaApp extends StatefulWidget {
  @override
  _MangaAppState createState() => _MangaAppState();
}

class _MangaAppState extends State<MangaApp> {

  void getData() async {

///   var asdas = await DriveService().uploadFile();
    ///
  //  var ImageBase64 = await ImageService().networkImageToBase64('https://cdn.pixabay.com/photo/2015/04/23/22/00/tree-736885__480.jpg');


//var string64 = await ImageService().networkImageToBase64('https://cdn.readonepiece.com/file/CDN-M-A-N/op_tcb_1049_004.png');

var sasda = ScrapeService().UpdateOnePieceTitles();
  }

  @override
  void initState()  {
    getData();
    super.initState();
    Flurorouter.setupRouter();



/*
ImageService().downloadwithdio("https://play-lh.googleusercontent.com/Jdnw_zqg__whDMACbqzV8Y-iftBh5-euDZtmxtgtfVTz_iBSf177xCbn37sFjdFRXvI",
    "C:/Users/rd28/Desktop/saveit/flutter/mymangawebsite/assets/topbar/gallery/current gallery/test 04.png");*/

//ImageService().downloadFile('https://play-lh.googleusercontent.com/Jdnw_zqg__whDMACbqzV8Y-iftBh5-euDZtmxtgtfVTz_iBSf177xCbn37sFjdFRXvI');

  }

  @override
  Widget build(BuildContext context) {



    return MaterialApp(
      initialRoute: '/',
      onGenerateRoute: Flurorouter.router.generator,
      theme: ThemeData(
          primarySwatch: Colors.green,
          scrollbarTheme: ScrollbarThemeData(
              isAlwaysShown: true,
              thickness: MaterialStateProperty.all(10),
              thumbColor: MaterialStateProperty.all(Colors.white70),
              radius: const Radius.circular(10),
              minThumbLength: 100)),
      home: Stack(
        children: const [
          ///child 01
          Scaffold(
              appBar: PreferredSize(
                  preferredSize: Size.fromHeight(150), child: MangaTopBar()),
              bottomNavigationBar: FooterWebsite()),

          Positioned(top: 138, child: BodyMangaSite()),
        ],
      ),
    );
  }
}



// now we have to initialise the router....

// thats it...all things are done now i think... :)
