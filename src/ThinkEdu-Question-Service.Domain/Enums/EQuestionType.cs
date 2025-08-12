namespace ThinkEdu_Question_Service.Domain.Enums
{
    public enum EQuestionType
    {
        MultipleChoice,   // Trắc nghiệm nhiều lựa chọn
        SingleChoice,     // Trắc nghiệm 1 lựa chọn
        Matching,         // ghép nối 
        FillInTheBlank,   // Điền vào chỗ trống
        TrueFalse,        // Đúng/Sai
        ShortAnswer,      // Tự luận ngắn
        Parent
    }
}