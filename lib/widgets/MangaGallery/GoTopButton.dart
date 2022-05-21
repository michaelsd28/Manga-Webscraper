import 'package:flutter/material.dart';

class GoTopButton extends StatelessWidget {

  final  ScrollController scrollToTopController;

  const GoTopButton( {Key? key, required this.scrollToTopController}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 400,
      child: Card(
          color: Colors.transparent,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Padding(
                  padding: EdgeInsets.all(10),
                  child: Row(
                    children: [
                      Padding(
                          padding: EdgeInsets.only(right: 10),
                          child: Text(
                            "Go to top",
                            style: TextStyle(fontSize: 20, color: Colors.white),
                          )),
                      TextButton(
                        style: ButtonStyle(
                          backgroundColor:
                          MaterialStateProperty.all<Color>(Colors.black26),
                          shape:
                          MaterialStateProperty.all<RoundedRectangleBorder>(
                            RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(85.0),
                            ),
                          ),
                        ),
                        child: Icon(Icons.arrow_upward),
                        onPressed: () {

                          scrollToTopController.animateTo(
                            0.0,
                            duration: Duration(milliseconds: 300),
                            curve: Curves.bounceInOut,
                          );

                        },
                      ),
                    ],
                  )),
            ],
          )),
    );
  }


}
