namespace BL.Models.FilesGeneration
{
    public class InflectedUkrText
    {
        public string Nominative { get; set; }

        public string Genitive { get; set; }
        
        public string Dative { get; set; }
        
        public string Accusative { get; set; }
        
        public string Instrumental { get; set; }
        
        public string Prepositional { get; set; }
        
        public string Vocative { get; set; }

        public override bool Equals(object obj)
        {
            bool result = true;
            
            if ( ! obj.GetType().IsAssignableTo(typeof(InflectedUkrText))) return false;

            InflectedUkrText inflectedText = (InflectedUkrText)obj;

            if ((this.Nominative != inflectedText.Nominative) ||
                (this.Accusative != inflectedText.Accusative) ||
                (this.Dative != inflectedText.Dative) ||
                (this.Genitive != inflectedText.Genitive) ||
                (this.Instrumental != inflectedText.Instrumental) ||
                (this.Prepositional != inflectedText.Prepositional) ||
                (this.Vocative != inflectedText.Vocative))
            {
                result = false;
            }
            

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
