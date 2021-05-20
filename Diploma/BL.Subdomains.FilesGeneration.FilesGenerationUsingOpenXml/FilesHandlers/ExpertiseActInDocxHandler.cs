﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using BL.Interfaces.Subdomains.FilesGeneration;
using DL.Entities.Enums;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers
{
    public class ExpertiseActInDocxHandler : IExpertiseActHandler
    {
        public FileType Type => FileType.ExpertiseAct;

        public FileFormat Format => FileFormat.DOCX;

        public string TemplateName { get; init; } = @"";

        #region Placeholders names in a template
        internal const string TEMPLATE_PLACEHOLDER_IN_TEMPLATE = @"";
        #endregion

        private readonly TemplateLoader _templateLoader;
        private readonly PartialTemplateFactory _partialTemplateFactory;

        public ExpertiseActInDocxHandler()
        {
            _templateLoader = new TemplateLoader();
            _partialTemplateFactory = new PartialTemplateFactory();
        }


        public async Task<FileModel> CreateFileAsync(SaveNoteOfAuthorsModel dataForCreating)
        {
            using var memStream = await _templateLoader.LoadTemplateAsync(TemplateName);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            SetAuthorsFullNameWithDegrees(wordDoc, dataForCreating.Authors);

            SetPublicationNameWithItsStatistic(wordDoc, dataForCreating.PublishingNameWithItsStatics);

            SetPublishingHouse(wordDoc, dataForCreating.PublishingHouse);

            SetNameOfUniversityDepartment(wordDoc, dataForCreating.UniversityDepartmentName);

            await SetDateAsync(wordDoc.MainDocumentPart.Document.Body, dataForCreating.PublishingDate);

            await SetAuthorsFullNameSignatureDateAsync(wordDoc.MainDocumentPart.Document.Body, dataForCreating.Authors,
                dataForCreating.PublishingDate);


            await SetFullNameSignatureDateOfChiefOfUniversityDepartmentAsync(wordDoc.MainDocumentPart.Document.Body,
                dataForCreating.FullNameOfChiefOfUniversityDepartment,
                dataForCreating.PublishingDate);

            // save wordDoc and get bytes from it
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
                author.Degrees.ForEach(d => authorFullNameWithDegrees = authorFullNameWithDegrees + d + ", ");

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

        /// <summary>
        /// Set date of creation document in template. This method doesn't work with document metadata;
        /// </summary>
        /// <returns></returns>
        private async Task SetDateAsync(Body docBody, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMyyyy, date);

            var nodeWithPlacelohderForDate = docBody.FindNodeWhichContainsText<OpenXmlElement>(
                DATE_PLACEHOLDER_IN_TEMPLATE);

            var nodeForInserting = nodeWithPlacelohderForDate;

            foreach (var node in partialTemplateNodes)
            {
                nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true));
            }

            nodeWithPlacelohderForDate.Remove();
        }

        private async Task SetAuthorsFullNameSignatureDateAsync(Body docBody, List<Author> authors, DateTime? date)
        {
            var nodeWithPlacelohderForAuthors = docBody.FindNodeWhichContainsText<OpenXmlElement>(
                AURHORS_FULL_NAME_SIGNATURE_DATE_PLACEHOLDER_IN_TEMPLATE);

            var nodeForInserting = nodeWithPlacelohderForAuthors;

            foreach (var author in authors)
            {
                var partialTemplateNodes =
                    await _partialTemplateFactory.GetFullNameSignatureDatePartialTemplateAsync(author.FullName, date);

                foreach (var node in partialTemplateNodes)
                {
                    nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true));
                }
            }

            nodeWithPlacelohderForAuthors.Remove();
        }

        private void SetNameOfUniversityDepartment(WordprocessingDocument wordDoc, string universityDepartmentName)
        {
            string universityDepartmentNameForTemplate = universityDepartmentName;

            // we should skip first word if this word is "кафедра"
            if (universityDepartmentName.ToLower().StartsWith("кафедра"))
            {
                universityDepartmentNameForTemplate =
                    universityDepartmentName.Substring(universityDepartmentName.IndexOf(' '));
            }

            wordDoc.ReplaceText(UNIVERSITY_DEPARTMENT_NAME_PLACEHOLDER_IN_TEMPLATE,
                universityDepartmentNameForTemplate.Trim(' ', ','),
                            false);
        }

        private async Task SetFullNameSignatureDateOfChiefOfUniversityDepartmentAsync(Body docBody, string fullNameOfChief, DateTime? dateTime)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetFullNameSignatureDatePartialTemplateAsync(fullNameOfChief, dateTime);

            // paragraph with placoholder for chief, it's required only for searching appropriate node for 
            // inserting paragraphs from partial template. It should be removed from final template
            var nodeWithPlacelohderForChief = docBody.FindNodeWhichContainsText<OpenXmlElement>(
                CHIEF_OF_UNIVERSITY_DEPARTMENT_FULLNAME_SIGNATURE_DATE_PLACEHOLDER_IN_TEMPLATE);

            var nodeForInserting = nodeWithPlacelohderForChief;

            foreach (var node in partialTemplateNodes)
            {
                nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true));
            }

            nodeWithPlacelohderForChief.Remove();
        }
    }
}
