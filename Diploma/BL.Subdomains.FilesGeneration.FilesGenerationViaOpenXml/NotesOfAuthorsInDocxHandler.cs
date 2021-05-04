using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using DL.Entities.Enums;
using DocumentFormat.OpenXml.Packaging;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml
{
    public class NotesOfAuthorsInDocxHandler : INotesOfAuthorsHandler
    {
        public string TemplateName { get; init; } = @"NotesOfAuthors.docx";

        public FileType Type => FileType.NoteOfAuthors;

        public FileFormat Format => FileFormat.DOCX;

        internal const string AUTHORS_FULL_NAME_PLACEHOLDER_IN_TEMPLATE = @"$AuthorsFullNameWithDegrees$";
        internal const string AURHORS_FULL_NAME_SIGNATURE_DATE_PLACEHOLDER_IN_TEMPLATE = @"$AuhtorsFullNameSignaruteDate$";
        internal const string PUBLISHING_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE = @"$PublishingNameWithItsStatistic$";
        internal const string PUBLISHING_HOUSE_NAME_PLACEHOLDER_IN_TEMPLATE = @"$PublishingHouseName$";
        internal const string CHIEF_OF_UNIVERSITY_DEPARTMENT_FULLNAME_SIGNATURE_DATE_PLACEHOLDER_IN_TEMPLATE = @"$ChiefOfUniverDepartmentFullNameSignaruteDate$";


        public async Task<FileModel> CreateFileAsync(SaveNoteOfAuthorsModel dataForCreating)
        {
            var templateLoader = new TemplateLoader();
             
            using var memStream = await templateLoader.LoadTemplateAsync(TemplateName);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);


            SetAuthorsFullNameWithDegrees(wordDoc, dataForCreating.Authors);

            SetPublicationNameWithItsStatistic(wordDoc, dataForCreating.PublishingNameWithItsStatics);

            SetPublishingHouse(wordDoc, dataForCreating.PublishingHouse);

            // todo : add other changes in file

            SetFullNameSignatureDateOfChiefOfUniversityDepartment(wordDoc,
                dataForCreating.FullNameOfChiefOfUniversityDepartment, 
                dataForCreating.PublishingDate);

            // save wordDoc and get bytes from it
            wordDoc.Package.Flush();
            wordDoc.Close();

            return new FileModel()
            {
                Format = this.Format,
                Type = this.Type,
                FileAsBytes = memStream.ToArray()
            };

        }

        private void SetAuthorsFullNameWithDegrees(WordprocessingDocument wordDoc, List<Author> authors)
        {
            var allAuthorsFullNameWithDegrees = new StringBuilder();

            foreach (var author in authors)
            {
                // example of string after this step : "Tymoshenko Oleh Oleksiiovych, "
                var authorFullNameWithDegrees = author.FullName + ", ";

                // example of string after this step : "Tymoshenko Oleh Oleksiiovych, PhD, docent, "
                author.Degrees.ForEach(d => authorFullNameWithDegrees = authorFullNameWithDegrees + d +  ", ");

                allAuthorsFullNameWithDegrees.Append(authorFullNameWithDegrees);
            }

            allAuthorsFullNameWithDegrees.ToString().Trim(' ', ',');


            wordDoc.ReplaceText(AUTHORS_FULL_NAME_PLACEHOLDER_IN_TEMPLATE,
                            allAuthorsFullNameWithDegrees.ToString().Trim(' ', ','),
                            false);
        }
        
        private void SetPublicationNameWithItsStatistic(WordprocessingDocument wordDoc, string publicationNameWithItsStatstics)
        {
            wordDoc.ReplaceText(PUBLISHING_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE,
                            publicationNameWithItsStatstics.Trim(' ', ','),
                            false);
        }

        private void SetPublishingHouse(WordprocessingDocument wordDoc, string publishingHouse)
        {
            wordDoc.ReplaceText(PUBLISHING_HOUSE_NAME_PLACEHOLDER_IN_TEMPLATE,
                            publishingHouse.Trim(' ', ','),
                            false);
        }

        private async void SetFullNameSignatureDateOfChiefOfUniversityDepartment(WordprocessingDocument wordDoc, string fullNameOfChief, DateTime dateTime)
        {
            var docBody = wordDoc.MainDocumentPart.Document.Body;
            PartialTemplateFactory partialTemplateFactory = new PartialTemplateFactory();

            var partialTemplateNodes = await partialTemplateFactory.GetFullNameSignatureDatePartialTemplateAsync(fullNameOfChief);

            // paragraph with placoholder for chief, it's required only for searching appropriate position(node) for 
            // inserting paragraphs from partial template. It should be removed from template
            var nodeWithPlacelohderForChief = docBody.ChildElements.FirstOrDefault(
                e => e.InnerText.Contains(CHIEF_OF_UNIVERSITY_DEPARTMENT_FULLNAME_SIGNATURE_DATE_PLACEHOLDER_IN_TEMPLATE));

            var nodeForInserting = nodeWithPlacelohderForChief;

            foreach (var node in partialTemplateNodes)
            {
                nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true));
            }

            nodeWithPlacelohderForChief.Remove();
        }

    }
}
