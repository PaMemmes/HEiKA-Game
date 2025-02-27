mergeInto(LibraryManager.library, {
  

  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },

  SpeakText: function (str) {
    var msg = new SpeechSynthesisUtterance();
    msg.text = UTF8ToString(str);
    msg.lang = 'de-DE';
    speechSynthesis.speak(msg);
  },

  PrintFloatArray: function (array, size) {
    for(var i = 0; i < size; i++)
    console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },

  ListenForSpeech: function(){
    class Spracherkennung {
        constructor() {
            this.recognition = new window.SpeechRecognition();
            this.recognition.onresult = this.handleRecognitionResult.bind(this);
            this.recognition.onerror = this.handleRecognitionError.bind(this);
          }

        start() {
            this.recognition.start();
          }

        stop() {
            this.recognition.stop();
          }

        handleRecognitionResult(event) {
            const transcript = event.results[0][0].transcript;
            console.log(`Erkannter Text: ${transcript}`);
            // Hier können weitere Aktionen basierend auf dem erkannten Text durchgeführt werden
          }

        handleRecognitionError(event) {
            console.error('Fehler bei der Spracherkennung:', event.error);
          }
        }
    const spracherkennung = new Spracherkennung();
    spracherkennung.start();
  }

});