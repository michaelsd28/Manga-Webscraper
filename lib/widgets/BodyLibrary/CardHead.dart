
import 'package:flutter/material.dart';

class CardHead extends StatelessWidget {

  final String mangaLogo;
  final List<Color> colors;

  const CardHead( {Key? key,required this.mangaLogo, required this.colors}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(

      height: 60,
      width: 150,
      decoration:   BoxDecoration(
          borderRadius: BorderRadius.circular(20),
          color: Colors.red,
          gradient:  LinearGradient(begin: Alignment.bottomRight, stops: const [
            0.05,
            0.9
          ], colors: colors.toList(),

          ),
          border: Border.all(
              color:Colors.black,
              width: 3

          )
      ),
      child:  Padding(
        padding: EdgeInsets.all(10),
        child: Image(
            image:  AssetImage(mangaLogo),


            colorBlendMode: BlendMode.modulate,
            filterQuality: FilterQuality.high
        ),
      ),

    );
  }
}