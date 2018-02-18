using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouseSystem.Camera.FaceIdentifyer
{
    public class FaceIdentifyer
    {
        const string friend1ImageDir = @"C:\Users\BartlomiejLeja\Pictures\TestPhotos\Magda";
        const string friend2ImageDir = @"C:\Users\BartlomiejLeja\Pictures\TestPhotos\Ola";
        const string friend3ImageDir = @"C:\Users\BartlomiejLeja\Pictures\TestPhotos\Barti";
        const string personGroupId = "test4";
        public async void TrainToIdnetifyFaces(FaceServiceClient faceServiceClient)
        {
           
            await faceServiceClient.CreatePersonGroupAsync(personGroupId, "Test");

            // Define Anna
            CreatePersonResult friend1 = await faceServiceClient.CreatePersonAsync(
                // Id of the person group that the person belonged to
                personGroupId,
                // Name of the person
                "Magda"
            );

            // Define Anna
            CreatePersonResult friend2 = await faceServiceClient.CreatePersonAsync(
                // Id of the person group that the person belonged to
                personGroupId,
                // Name of the person
                "Ola"
            );

            CreatePersonResult friend3 = await faceServiceClient.CreatePersonAsync(
               // Id of the person group that the person belonged to
               personGroupId,
               // Name of the person
               "Barti"
           );

            foreach (string imagePath in Directory.GetFiles(friend1ImageDir, "*.jpg"))
            {
                
                await Task.Run(async () =>
                {
                    using (Stream s = File.OpenRead(imagePath))
                    {
                      //  Detect faces in the image and add to Anna
    
                        await faceServiceClient.AddPersonFaceAsync(
                            personGroupId, friend1.PersonId, s);
                        Debug.WriteLine("Załadowana Magda");
                    }
                });
            }

            foreach (string imagePath in Directory.GetFiles(friend2ImageDir, "*.jpg"))
            {
                await Task.Run(async () =>
                {
                    using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await faceServiceClient.AddPersonFaceAsync(
                        personGroupId, friend2.PersonId, s);
                        Debug.WriteLine("Załadowana Ola");
                    }
                });
            }

            foreach (string imagePath in Directory.GetFiles(friend3ImageDir, "*.jpg"))
            {
                await Task.Run(async () =>
                { 
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await faceServiceClient.AddPersonFaceAsync(
                        personGroupId, friend3.PersonId, s);
                        Debug.WriteLine("Załadowana Bart");
                    }
                });
            }
            await faceServiceClient.TrainPersonGroupAsync(personGroupId);
            TrainingStatus trainingStatus = null;
            while (true)
            {
                try
                {
                    trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(personGroupId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                if (trainingStatus.Status != Status.Running )
                {
                    break;
                }

                await Task.Delay(1000);
            }
            Debug.WriteLine("End");
        }

        public async Task<string> RecognizeFaces(FaceServiceClient faceServiceClient, Face [] faces)
        {
            
            var faceIds = faces.Select(face => faces[0].FaceId).ToArray();

            var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
            foreach (var identifyResult in results)
            {
                Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                if (identifyResult.Candidates.Length == 0)
                {
                    Debug.WriteLine("No one identified");
                   return "No one identified";
                }
                else
                {
                    // Get top 1 among all candidates returned
                    var candidateId = identifyResult.Candidates[0].PersonId;
                    var person = await faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                    Debug.WriteLine("Identified as {0}", person.Name);
                    return $"{person.Name}";
                }
            }

            return "No one identified";
        }

    }
}
