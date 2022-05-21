
import 'package:flutter/material.dart';
import 'package:mymangawebsite/Routes.dart';

class MangaTopBar extends StatelessWidget {

  @override
  Size get preferredSize => const Size.fromHeight(300);

  const MangaTopBar({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      automaticallyImplyLeading: false,
      toolbarHeight: 150.0,
      centerTitle: true,
      title:  const Center(
          child: TopButton(),

          ),


      backgroundColor: Colors.blueGrey[900],
      flexibleSpace: const Image(
        image:  AssetImage('assets/topbar/landing.jpg'),
        color: Color.fromRGBO(255, 255, 255, 0.2),
        colorBlendMode: BlendMode.modulate,
        fit: BoxFit.cover,
      ),

    );


  }
}



//// manga website text and button
class TopButton extends StatelessWidget {
  const TopButton({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return TextButton(

        onPressed: () {

          /// clear the stack and go to the home page




         Navigator.pushNamed(context,"/");




        },
        style: ButtonStyle(
          backgroundColor: MaterialStateProperty.all<Color>(Colors.transparent),
          foregroundColor: MaterialStateProperty.all<Color>(Colors.red),
          overlayColor: MaterialStateProperty.all<Color>(Color.fromRGBO(144, 43, 42, 0.1),),
          shape: MaterialStateProperty.all<RoundedRectangleBorder>(RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(15.0),
          ),),
        ), child:       const Text('Manga Website',
      style: TextStyle(fontSize: 20),)
    );
  }
}
