from hezar.models import Model

model = Model.load("hezarai/crnn-fa-64x256-license-plate-recognition")

def extract(filePath):
    return model.predict(filePath)
