﻿namespace Domain.Entities;

public class QuestionDto
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Text { get; set; }
    public required string? Description { get; set; }
    public required object SpecificData { get; set; } // Данные специфичны для каждого типа 
    //todo не уверен, что возвращать через object хорошая идея, нужно обсудить с фронтом как удобнее будет =/
    //todo можно вернуть тупо объекты замапенные под интерфейс и норм, но как со стороны фронта эта будет? Ему то придут нужные объекты, но удобнее ли это? Чем мы сделаем через object и тип
    //todo я чет пытался в репозитории, пока оставил код там, чтобы не через обджект, надо доделывать, сейчас самая сложная. Я всё-таки думаю, что не стоит все пытаться впихнуть в одно, и сделать дтошку, где можно впихивать в разные поля вопросики,
    //todo как я показывал тебе, но вопрос в последовательности итп остается. Значит нужно и приоритет у них делать итп, крч надо обсудить.
    //todo я полностью перепимсываю архитектуру тестов
}